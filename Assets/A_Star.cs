using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star : MonoBehaviour
{

    Grid grid;



    public Transform target;
    public Transform start;


    private void Start() {
        grid = GetComponent<Grid>();
        openList = new List<Node>();
        closedList = new List<Node>();
        path = new List<Node>();
    }
    private void Update() {

        FindPath(start.position, target.position);

    }

    public List<Node> openList;
    public List<Node> closedList;

    public void FindPath(Vector3 startPos, Vector3 endPos) {

        var start = grid.NodeFromWorldPoint(startPos);
        var end = grid.NodeFromWorldPoint(endPos);

        //closedList.Clear();
        //openList.Clear();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(start);

        while (openList.Count > 0) {

            Node current = openList[0];
            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].fCost < current.fCost || openList[i].fCost == current.fCost && openList[i].hCost < current.hCost) {
                    current = openList[i];
                }
            }

            openList.Remove(current);
            closedList.Add(current);

            if (current == end) {
                //retrace
                Retrace(start, end);
                //Debug.Log("Done");
                return;
            }


            foreach (var neighbour in grid.GetNeighbours(current)) {
                if (closedList.Contains(neighbour) || !neighbour.walkable) {
                    continue;
                }

                int newG = current.gCost + Distance(current, neighbour);
                if (newG < neighbour.gCost || !openList.Contains(neighbour)) {
                    neighbour.gCost = newG;
                    neighbour.hCost = Distance(neighbour, end);
                    neighbour.parent = current;
                    if (!openList.Contains(neighbour)) {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        //Retrace(start, end);
        //Debug.Log("No Solution");
    }
 
   
    int Distance(Node a, Node b) {

        Vector2Int d = Grid.Abs(a.gridIndex - b.gridIndex);

        if (d.x > d.y) // Try to take as many diagonal steps as possible (limited by the shortest one)
            return 14 * d.y + 10 * (d.x - d.y);
        return 14 * d.x + 10 * (d.y - d.x);
    }


    List<Node> path;
    void Retrace(Node start, Node end) {

        var current = end;
        while (current != start) {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        grid.testPath = path;
        grid.closedPath = closedList;
    }
}



/*

// A* finds a path from start to goal.
// h is the heuristic function. h(n) estimates the cost to reach goal from node n.
function A_Star(start, goal, h)
    // The set of discovered nodes that may need to be (re-)expanded.
    // Initially, only the start node is known.
    // This is usually implemented as a min-heap or priority queue rather than a hash-set.
openSet:= { start}

// For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start
// to n currently known.
cameFrom:= an empty map

    // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
    gScore := map with default value of Infinity
    gScore[start] := 0

    // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
    // how short a path from start to finish can be if it goes through n.
fScore:= map with default value of Infinity
    fScore[start] := h(start)

    while openSet is not empty
        // This operation can occur in O(1) time if openSet is a min-heap or a priority queue
        current := the node in openSet having the lowest fScore[] value
        if current = goal
            return reconstruct_path(cameFrom, current)

        openSet.Remove(current)
        for each neighbor of current
            // d(current,neighbor) is the weight of the edge from current to neighbor
            // tentative_gScore is the distance from start to the neighbor through current
            tentative_gScore := gScore[current] + d(current, neighbor)
            if tentative_gScore < gScore[neighbor]
                // This path to neighbor is better than any previous one. Record it!
                cameFrom[neighbor] := current
                gScore[neighbor] := tentative_gScore
                fScore[neighbor] := gScore[neighbor] + h(neighbor)
                if neighbor not in openSet
                    openSet.add(neighbor)

    // Open set is empty but goal was never reached
return failure


*/