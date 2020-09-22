using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;


public class Grid : MonoBehaviour
{
    public bool showClosedList = false;
    public Transform[] staticObjects;
    public Transform[] CostObjects;
    public int[] costs;
    public Node[,] grid;

    public Vector2Int gridWorldsize;
    [Min(float.Epsilon)]
    public float resolution; // 1/x

    float nodeRadius;
    float nodeDiameter;

    Vector2Int gSize;

    A_Star a;

    private void Start() {

        CreateGrid();
        a = GetComponent<A_Star>();
    }


    void CreateGrid() {

        nodeDiameter = 1 / resolution;
        nodeRadius = nodeDiameter / 2;
        gSize = Vector2Int.RoundToInt((Vector2)gridWorldsize / nodeDiameter);
        grid = new Node[gSize.x, gSize.y];

        for (int i = 0; i < gSize.x; i++) {
            for (int k = 0; k < gSize.y; k++) {

                Vector3 pos = Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (k * nodeDiameter + nodeRadius); // in the middle of each node also avoid "new" keyword
                grid[i, k] = new Node(pos, true, new Vector2Int(i,k));
            }
        }

        //Calculate obstacles
        foreach (var obj in staticObjects) {
            CalculateStaticObject(obj.position, obj.localScale);
        }

        for (int i = 0; i < CostObjects.Length; i++) {
            CalculateStaticObject(CostObjects[i].position, CostObjects[i].localScale, true, costs[i]);
        }
    }

    public Transform testCube;
    private void Update() {

        CalculateStaticObject(testCube.position, testCube.localScale); // active 




        // DEbug
        if (Input.GetKeyDown(KeyCode.Space)) {
            CreateGrid();
        }

    }

    private void CalculateStaticObject(Vector3 position, Vector3 size, bool walkable = false, int startCost = 0) {

        Vector3 lowerPos = position - size / 2;
        Vector3 upperPos = position + size / 2;

        Vector2Int lowerbound = GetNodeIndex(lowerPos);
        Vector2Int upperbound = GetNodeIndex(upperPos);

        Vector2Int gridLimits = new Vector2Int(grid.GetLength(0), grid.GetLength(1));
        lowerbound.Clamp(Vector2Int.zero, gridLimits);
        upperbound.Clamp(Vector2Int.zero, gridLimits);

        for (int i = lowerbound.x; i < upperbound.x; i++) {
            for (int k = lowerbound.y; k < upperbound.y; k++) {
                grid[i, k].walkable = walkable;
                grid[i, k].startCost = startCost;
            }
        }
    }

    public Vector2Int GetNodeIndex(Vector3 pos) {
        return XYZtoXZ(Vector3Int.RoundToInt((pos - Vector3.one * nodeRadius) / nodeDiameter));// change to floor?;
    }
    public Node GetNode(Vector3 pos) {
        return grid[GetNodeIndex(pos).x, GetNodeIndex(pos).y];
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = worldPosition.x  / gridWorldsize.x;
        float percentY = worldPosition.z  / gridWorldsize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((gSize.y - 1) * percentY);
        return grid[x, y];
    }
    
    public List<Node> GetNeighbours(Node node) {

        List<Node> neighbours = new List<Node>();
        for (int i = -1; i <= 1; i++) {
            for (int k = -1; k <= 1; k++) {
                if (i == 0 && k == 0) // Skip Middle node
                    continue;

                Vector2Int index = node.gridIndex + new Vector2Int(i, k);
                if (index.x >= 0 && index.x < gSize.x) {
                    if (index.y >= 0 && index.y < gSize.y) {
                        neighbours.Add(grid[index.x, index.y]);
                    }
                }

            }
        }
        return neighbours;
    }
    
    public List<Node> testPath = new List<Node>();

    public List<Node> closedPath = new List<Node>();

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(new Vector3(gridWorldsize.x / 2f, 0, gridWorldsize.y / 2f), new Vector3(gridWorldsize.x, 2, gridWorldsize.y));


        if (showClosedList) {
            if (closedPath != null) {

                foreach (var node in closedPath) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(node.worldPositon, Vector3.one * nodeRadius / 1.5f);
                }
            }
        }


        if (grid != null) {
            foreach (var node in grid) {
                    if (node.walkable) {
                        if (testPath != null) {
                            if (testPath.Contains(node)) {
                                Gizmos.color = Color.black;
                                Gizmos.DrawSphere(node.worldPositon, nodeRadius / 2);
                            }
                        }


                        Gizmos.color = Color.white;
                    

                    if (node.startCost > 10) {
                        Gizmos.color = Color.yellow;
                    }
                    Gizmos.DrawCube(node.worldPositon, Vector3.one * nodeRadius / 2);
                    }
                    else {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(node.worldPositon, Vector3.one * nodeRadius / 2);

                    }

            }
        }

    }



    public static Vector2Int XYZtoXZ(Vector3Int v) => new Vector2Int(v.x, v.z);

    public static Vector2Int Abs(Vector2Int v) => new Vector2Int(Mathf.Abs(v.x), Mathf.Abs(v.y));

}
