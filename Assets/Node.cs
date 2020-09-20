using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPositon;

    public Node(Vector3 _worldPosition, bool isWalkable) {
        worldPositon = _worldPosition;
        walkable = isWalkable;
    }

}
