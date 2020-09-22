using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPositon;
    public Vector2Int gridIndex;

    public Node(Vector3 _worldPosition, bool isWalkable, Vector2Int gridIndex) {
        worldPositon = _worldPosition;
        walkable = isWalkable;
        this.gridIndex = gridIndex;
    }

    public Node parent;
    public int gCost;
    public int hCost;
    public int startCost = 0;


    public int fCost {
        get {
            return hCost + gCost + startCost;
        }
    }


}
