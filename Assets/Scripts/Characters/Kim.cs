using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Kim : CharacterController
{
    [SerializeField] float ContextRadius;

    public override void StartCharacter()
    {
        base.StartCharacter();
    }

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();
        Debug.Log($"Walk Buffer Count: {myWalkBuffer.Count}, Reached Destination: {myReachedDestination}");
        Zombie closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();
       
          if (myWalkBuffer.Count == 0 && myReachedDestination)
        {

            List<Grid.Tile> path = FindPath(Grid.Instance.GetClosest(transform.position), Grid.Instance.GetFinishTile());
        Debug.Log("logggg");
            SetWalkBuffer(path);
        }
    }

    Vector3 GetEndPoint()
    {
        return Grid.Instance.WorldPos(Grid.Instance.GetFinishTile());
    }

    GameObject[] GetContextByTag(string aTag)
    {
        Collider[] context = Physics.OverlapSphere(transform.position, ContextRadius);
        List<GameObject> returnContext = new List<GameObject>();
        foreach (Collider c in context)
        {
            if (c.transform.CompareTag(aTag))
            {
                returnContext.Add(c.gameObject);
            }
        }
        return returnContext.ToArray();
    }

    GameObject GetClosest(GameObject[] aContext)
    {
        float dist = float.MaxValue;
        GameObject Closest = null;
        foreach (GameObject z in aContext)
        {
            float curDist = Vector3.Distance(transform.position, z.transform.position);
            if (curDist < dist)
            {
                dist = curDist;
                Closest = z;
            }
        }
        return Closest;
    }
    public List<Grid.Tile> FindPath(Grid.Tile startTile, Grid.Tile endTile)
    {
        List<Grid.Tile> openList = new List<Grid.Tile>(); // tiles to be evaluated
        HashSet<Grid.Tile> closedList = new HashSet<Grid.Tile>(); // already evaluated tiles

        Dictionary<Grid.Tile, Grid.Tile> parentMap = new Dictionary<Grid.Tile, Grid.Tile>(); // to track parent nodes for path reconstruction

        Dictionary<Grid.Tile, float> gCost = new Dictionary<Grid.Tile, float>(); // movement cost from start to this tile
        Dictionary<Grid.Tile, float> fCost = new Dictionary<Grid.Tile, float>(); // gCost + hCost

        openList.Add(startTile);
        gCost[startTile] = 0;
        fCost[startTile] = GetHeuristic(startTile, endTile);

        while (openList.Count > 0)
        {
            // Sort the open list by fCost and pick the tile with the lowest value
            Grid.Tile currentTile = openList.OrderBy(t => fCost.ContainsKey(t) ? fCost[t] : float.MaxValue).First();

            if (currentTile == endTile)
            {
                // Reconstruct the path by tracing parents
                return ReconstructPath(parentMap, currentTile);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (Grid.Tile neighbor in GetNeighbors(currentTile))
            {
                if (neighbor.occupied || closedList.Contains(neighbor))
                {
                    // Skip occupied or already evaluated tiles
                    continue;
                }

                float tentativeGCost = gCost[currentTile] + GetDistance(currentTile, neighbor);

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGCost >= gCost[neighbor])
                {
                    // Skip if this path isn't better
                    continue;
                }

                // Update the parent map, gCost and fCost
                parentMap[neighbor] = currentTile;
                gCost[neighbor] = tentativeGCost;
                fCost[neighbor] = gCost[neighbor] + GetHeuristic(neighbor, endTile);
            }
        }

        // If no path is found, return null or an empty list
        return new List<Grid.Tile>();
    }
    private float GetHeuristic(Grid.Tile a, Grid.Tile b)
    {
        // Manhattan distance heuristic
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
        path.Reverse(); // We built the path backward, so reverse it
        return path;
    }
    private List<Grid.Tile> GetNeighbors(Grid.Tile tile)
    {
        List<Grid.Tile> neighbors = new List<Grid.Tile>();

        Vector2Int[] neighborOffsets = new Vector2Int[]
        {
        new Vector2Int(0, 1), // Up
        new Vector2Int(0, -1), // Down
        new Vector2Int(1, 0), // Right
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 1),   // Up-Right
        new Vector2Int(1, -1),  // Down-Right
        new Vector2Int(-1, 1),  // Up-Left
        new Vector2Int(-1, -1)  // Down-Left
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
