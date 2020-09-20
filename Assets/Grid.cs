using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Node[,] nodes;

    public Vector2Int gridWorldsize;
    public float resolution; // 1/x

    public float nodeRadius;
    float nodeDiameter;

    int gridsizeX, gridsizeY;

    private void Start() {

        nodeDiameter = nodeRadius * 2;

        gridsizeX = Mathf.RoundToInt(gridWorldsize.x / nodeDiameter);
        gridsizeY = Mathf.RoundToInt(gridWorldsize.y / nodeDiameter);
    }




    void OnDrawGizmos() { // use only x -> x%2 = 0 or x& binary
        Gizmos.DrawWireCube(new Vector3(gridWorldsize.x/2f, 0, gridWorldsize.y/2f), new Vector3(gridWorldsize.x, 2, gridWorldsize.y));
    }


}
