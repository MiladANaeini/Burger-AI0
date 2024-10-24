using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindBurgerPathTask : Node
{
    private Kim myKim;
    private bool pathFound = false;
    private List<GameObject> burgers;

    public FindBurgerPathTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }

    public override ReturnState EvaluateState()
    {
        if (pathFound)
        {
            Debug.Log("Burger path already found, success.");
            return ReturnState.s_Failure;  // Return failure to move to the next task after the burger is collected
        }

        BlackBoard blackboard = myKim.blackboard;

        // Get all burgers in the scene (assuming they are tagged as "Burger")
        if (burgers == null || burgers.Count == 0)
        {
            burgers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Burger"));
        }

        if (burgers.Count == 0)
        {
            Debug.Log("No more burgers found, moving to finish.");
            return ReturnState.s_Success;
        }

        GameObject closestBurger = GetClosestBurger();
        if (closestBurger == null)
        {
            Debug.Log("Couldn't find a path to the burger, failing.");
            return ReturnState.s_Success;
        }

        // Find the path to the closest burger
        List<Grid.Tile> newPath = FindPath(Grid.Instance.GetClosest(myKim.transform.position), Grid.Instance.GetClosest(closestBurger.transform.position));

        if (newPath == null || newPath.Count == 0)
        {
            Debug.Log("Pathfinding to burger failed.");
            return ReturnState.s_Success;
        }
        // Store the path in the blackboard
        blackboard.Data.Remove("path");
        blackboard.Data["path"] = newPath;
        myKim.SetWalkBuffer(newPath);

        Debug.Log("New burger path found and stored.");

        // Remove the collected burger from the list without destroying it
        burgers.Remove(closestBurger);

        pathFound = true;  // Cache the result so this task doesn't run again for the same burger
        return ReturnState.s_Failure;  // Task complete, return failure to move to next task
    }

    public override void Reset()
    {
        base.Reset();
        pathFound = false;  // Reset the cached path when the task is reset
    }

    // Helper function to find the closest burger
    private GameObject GetClosestBurger()
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject burger in burgers)
        {
            if (burger == null) continue; // Check if burger still exists

            float distance = Vector3.Distance(myKim.transform.position, burger.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = burger;
            }
        }

        return closest;
    }

    // You can use your existing pathfinding logic here to find the path
    private List<Grid.Tile> FindPath(Grid.Tile startTile, Grid.Tile endTile)
    {
        List<Grid.Tile> openList = new List<Grid.Tile>();
        HashSet<Grid.Tile> closedList = new HashSet<Grid.Tile>();
        Dictionary<Grid.Tile, Grid.Tile> parentMap = new Dictionary<Grid.Tile, Grid.Tile>();
        Dictionary<Grid.Tile, float> gCost = new Dictionary<Grid.Tile, float>();
        Dictionary<Grid.Tile, float> fCost = new Dictionary<Grid.Tile, float>();

        openList.Add(startTile);
        gCost[startTile] = 0;
        fCost[startTile] = GetHeuristic(startTile, endTile);

        while (openList.Count > 0)
        {
            Grid.Tile currentTile = openList.OrderBy(t => fCost.ContainsKey(t) ? fCost[t] : float.MaxValue).First();

            if (currentTile == endTile)
            {
                return ReconstructPath(parentMap, currentTile);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (Grid.Tile neighbor in GetNeighbors(currentTile))
            {
                if (neighbor.occupied || closedList.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGCost = gCost[currentTile] + GetDistance(currentTile, neighbor);

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGCost >= gCost[neighbor])
                {
                    continue;
                }

                parentMap[neighbor] = currentTile;
                gCost[neighbor] = tentativeGCost;
                fCost[neighbor] = gCost[neighbor] + GetHeuristic(neighbor, endTile);
            }
        }

        return new List<Grid.Tile>(); // Placeholder
    }
    private float GetHeuristic(Grid.Tile a, Grid.Tile b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Grid.Tile> ReconstructPath(Dictionary<Grid.Tile, Grid.Tile> parentMap, Grid.Tile currentTile)
    {
        List<Grid.Tile> path = new List<Grid.Tile>();
        while (parentMap.ContainsKey(currentTile))
        {
            path.Add(currentTile);
            currentTile = parentMap[currentTile];
        }
        path.Reverse();
        return path;
    }
    private List<Grid.Tile> GetNeighbors(Grid.Tile tile)
    {
        List<Grid.Tile> neighbors = new List<Grid.Tile>();

        Vector2Int[] neighborOffsets = new Vector2Int[]
        {
            new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1)
        };

        foreach (Vector2Int offset in neighborOffsets)
        {
            Grid.Tile neighborTile = Grid.Instance.TryGetTile(new Vector2Int(tile.x + offset.x, tile.y + offset.y));
            if (neighborTile != null)
            {
                neighbors.Add(neighborTile);
            }
        }

        return neighbors;
    }
    private float GetDistance(Grid.Tile a, Grid.Tile b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return dx > dy ? 1.41f * dy + 1 * (dx - dy) : 1.41f * dx + 1 * (dy - dx);
    }
}
