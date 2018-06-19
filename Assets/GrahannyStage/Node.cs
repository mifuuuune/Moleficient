using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public float gCost;
    public float hCost;

    //public float distance;
    //public float estimate;
    public Node parent;

    public int gridX;
    public int gridY;

    public bool walkable;
    public Vector3 worldPosition;

    public Node(bool w, Vector3 wp, int gx, int gy)
    {
        walkable = w;
        worldPosition = wp;
        gridX = gx;
        gridY = gy;
    }

    public string toDebugString()
    {
        return "[" + gridX + "," + gridY + "] -> parent : [" + (parent == null ? "null" : (parent.gridX + "," + parent.gridY)) + "]";
    }

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }

    }

}
