using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Node[,] grid;

    public Vector2Int gridWorldsize;
    public float resolution; // 1/x

    public float nodeRadius;
    float nodeDiameter;

    int gridsizeX, gridsizeY;

    private void Start() {

        nodeDiameter = nodeRadius * 2;

        gridsizeX = Mathf.RoundToInt(gridWorldsize.x / nodeDiameter);
        gridsizeY = Mathf.RoundToInt(gridWorldsize.y / nodeDiameter);

        CreateGrid();
    }


    void CreateGrid() {
        grid = new Node[gridsizeX, gridsizeY];
        //Vector3 anchorPoint = new Vector3(gridWorldsize.x / 2f, 0, gridWorldsize.y / 2f);

        for (int i = 0; i < gridsizeX; i++) {
            for (int k = 0; k < gridsizeY; k++) {

                Vector3 pos = Vector3.right * (i + nodeRadius) + Vector3.forward * (k + nodeRadius); // in the middle of each node
                grid[i, k] = new Node(pos, true);
            }
        }


    }



    void OnDrawGizmos() { // use only x -> x%2 = 0 or x& binary
        Gizmos.DrawWireCube(new Vector3(gridWorldsize.x/2f, 0, gridWorldsize.y/2f), new Vector3(gridWorldsize.x, 2, gridWorldsize.y));


        if (grid != null) {
            foreach (var node in grid) {
                Gizmos.DrawCube(node.worldPositon, Vector3.one * 0.2f);
            }
        }

    }


}
