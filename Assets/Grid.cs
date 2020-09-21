using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;


public class Grid : MonoBehaviour
{

    public Node[,] grid;

    public Vector2Int gridWorldsize;
    [Min(float.Epsilon)]
    public float resolution; // 1/x

    float nodeRadius;
    float nodeDiameter;

    int gridsizeX, gridsizeY;

    private void Start() {

        nodeDiameter = 1 / resolution;
        nodeRadius = nodeDiameter / 2;

        gridsizeX = Mathf.RoundToInt(gridWorldsize.x / nodeDiameter);
        gridsizeY = Mathf.RoundToInt(gridWorldsize.y / nodeDiameter);

        CreateGrid();
    }


    void CreateGrid() {
        grid = new Node[gridsizeX, gridsizeY];
        //Vector3 anchorPoint = new Vector3(gridWorldsize.x / 2f, 0, gridWorldsize.y / 2f);

        for (int i = 0; i < gridsizeX; i++) {
            for (int k = 0; k < gridsizeY; k++) {

                Vector3 pos = Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (k * nodeDiameter + nodeRadius); // in the middle of each node also avoid "new" keyword
                grid[i, k] = new Node(pos, true);
                if (i == 3 && k  == 5) {
                    grid[i, k] = new Node(pos, false);

                }
            }
        }

        //x
        // i * nodeDiameter + noderadius
        //(pos - node radius)/nodeDiameter -> i // round i? 

    }

    public Transform testCube;
    private void Update() {
        Vector3 pos = testCube.position;
        
        Vector3 size = testCube.localScale;


        Vector3 lowerPos = pos - size / 2;
        Vector3 upperPos = pos + size / 2;

        Vector3Int lowerbound = Vector3Int.RoundToInt((lowerPos - Vector3.one * nodeRadius) / nodeDiameter);
        Vector3Int upperbound = Vector3Int.RoundToInt((upperPos - Vector3.one * nodeRadius) / nodeDiameter);

        


        for (int i = lowerbound.x; i < upperbound.x; i++) {
            for (int k = lowerbound.z; k < upperbound.z; k++) {
                Debug.Log(i + " d " + k);
                grid[i,k].walkable = false;
            }
        }




        int x = Mathf.RoundToInt((pos.x - nodeRadius) / nodeDiameter);
        int z = Mathf.RoundToInt((pos.z - nodeRadius) / nodeDiameter);

        Vector3Int bound = Vector3Int.RoundToInt((pos - Vector3.one * nodeRadius) / nodeDiameter);

        grid[bound.x, bound.z].walkable = false;



        // DEbug
        if (Input.GetKeyDown(KeyCode.Space)) {
            CreateGrid();
        }

    }

    void OnDrawGizmos() { // use only x -> x%2 = 0 or x& binary
        Gizmos.DrawWireCube(new Vector3(gridWorldsize.x/2f, 0, gridWorldsize.y/2f), new Vector3(gridWorldsize.x, 2, gridWorldsize.y));


        if (grid != null) {
            foreach (var node in grid) {
                if (node.walkable) {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(node.worldPositon, Vector3.one * nodeRadius/2);
                }
                else {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(node.worldPositon, Vector3.one * nodeRadius / 2);

                }
            }
        }

    }



    public static Vector3 Abs(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

}
