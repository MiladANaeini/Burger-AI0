using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FindPathTask : Node
{
    private Kim myKim;
    private bool pathFound = false;
    public FindPathTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;

    }

    public override ReturnState EvaluateState()
    {
        if (pathFound)
        {
            Debug.Log("Path already found, success.");
            return ReturnState.s_Failure; // Return cached success state
        }

        BlackBoard blackboard = myKim.blackboard;

        if (blackboard.Data.ContainsKey("zombieDetected") && (bool)blackboard.Data["zombieDetected"])
        {
            Debug.Log("Zombie detected, fail.");
            return ReturnState.s_Success;
        }

        if (blackboard.Data.ContainsKey("path") && blackboard.Data["path"] is List<Grid.Tile> path && path.Count > 0)
        {
            Debug.Log("Path already found in blackboard, success.");
            pathFound = true; // Cache it in
            return ReturnState.s_Failure;
        }

        List<Grid.Tile> newPath = FindPath(Grid.Instance.GetClosest(myKim.transform.position), Grid.Instance.GetFinishTile());

        if (newPath == null || newPath.Count == 0)
        {
            Debug.Log("Pathfinding failed.");
            return ReturnState.s_Success;
        }

        blackboard.Data["path"] = newPath;
        Debug.Log("New path found and stored.");
        pathFound = true; // Cache the result so this task doesn't run again
        return ReturnState.s_Failure;
    }

    public override void Reset()
    {
        base.Reset();
        pathFound = false; // Reset cached result when the behavior tree is reset
    
     }
 
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

        return new List<Grid.Tile>(); // No path found
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
