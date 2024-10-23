using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GoAroundZombieTask : Node
{
    private Kim myKim;
    public GoAroundZombieTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }
    public override ReturnState EvaluateState()
    {
        BlackBoard blackboard = myKim.blackboard;

        if (!blackboard.Data.ContainsKey("zombieDetected") || !(bool)blackboard.Data["zombieDetected"])
        {
            return ReturnState.s_Failure;
        }

        Grid.Tile startTile = Grid.Instance.GetClosest(myKim.transform.position);
        Grid.Tile endTile = Grid.Instance.GetFinishTile();

        List<Grid.Tile> newPath = FindPathWithZombieAvoidance(startTile, endTile);

        if (newPath == null || newPath.Count == 0)
        {
            Debug.Log("Failed to find a path around zombies.");
            return ReturnState.s_Failure; // Pathfinding failed
        }

        blackboard.Data["path"] = newPath;
        Debug.Log("New path avoiding zombies stored in blackboard.");

        return ReturnState.s_Failure;  
    }
    private List<Grid.Tile> FindPathWithZombieAvoidance(Grid.Tile startTile, Grid.Tile endTile)
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

                float zombiePenalty = GetZombiePenalty(neighbor);
                float tentativeGCost = gCost[currentTile] + GetDistance(currentTile, neighbor) + zombiePenalty;

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

        return new List<Grid.Tile>();
    }
    // Same helper methods as in FindPathTask
    private float GetZombiePenalty(Grid.Tile tile)
    {
        float penalty = 5; // Base penalty value

        Collider[] colliders = Physics.OverlapSphere(Grid.Instance.WorldPos(tile), 1.0f); // Adjust radius as needed
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Zombie"))
            {
                float distanceToZombie = Vector3.Distance(Grid.Instance.WorldPos(tile), collider.transform.position);
                penalty += Mathf.Clamp(10 - distanceToZombie, 0, 10); // Max penalty of 10
            }
        }

        return penalty;
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

