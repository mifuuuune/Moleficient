using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pathfinding : MonoBehaviour  {

    //public Transform seeker, target;
    public Vector3 startingPoint;
    public Vector3 target;
    public Vector3 finalPoint;
    private Grid grid;
    public List<Node> path;
    public Color color;

    //List<Node> visited;
    //List<Node> unvisited;
/*
    private void Awake()
    {
        //grid = GameObject.Find("A*").GetComponent<Grid>();
    }
    */
    public void askGrid()
    {
        grid = GameObject.Find("A*").GetComponent<Grid>();
    }


    void Findpath(Vector3 startPos, Vector3 targetPos)
    {
        if (grid)
        {
            //Debug.Log("sono nel grid");
            Node startNode = grid.getNodeFromWorldPoint(startPos);
            Node targetNode = grid.getNodeFromWorldPoint(targetPos);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    retracePath(startNode, targetNode);
                    return;
                }

                foreach (Node n in grid.getNeighbours(currentNode))
                {
                    if (!n.walkable || closedSet.Contains(n)) continue;

                    float costToNeighbour = currentNode.gCost + getDistance(currentNode, n);
                    if (costToNeighbour < n.gCost || !openSet.Contains(n))
                    {
                        n.gCost = costToNeighbour;
                        n.hCost = getDistance(n, targetNode);
                        n.parent = currentNode;

                        if (!openSet.Contains(n))
                        {
                            openSet.Add(n);
                        }
                    }
                }
            }
        }
        

        /*Node start = grid.getNodeFromWorldPoint(startPos);
        Node goal = grid.getNodeFromWorldPoint(targetPos);

        visited = new List<Node>();
        unvisited = grid.getNodes();

        foreach (Node n in unvisited)
        {
            n.distance = (n == start ? 0f : float.MaxValue);
            n.estimate = (n == goal ? 0f : float.MaxValue);
        }

        while (goal.distance == float.MaxValue)
        {
            Node current = getNextNode();

            if (current == goal)
            {
                //Debug.Log("start: " + start  + ", goal: " + goal);
                retracePath(start, current);
                return;
            }
            
            if (current.distance == float.MaxValue)
            {
                Debug.Log("graph partitioned");
                break;
            }

            foreach (Node n in grid.getNeighbours(current))
            {
                if (current.distance + getDistance(current, n) < n.distance)
                {
                    n.distance = current.distance + getDistance(current, n);
                    n.estimate = n.distance + getDistance(n, goal);
                    n.parent = current;

                    if (visited.Contains(n))
                    {
                        unvisited.Add(n);
                        visited.Remove(n);
                    }
                }
            }
            visited.Add(current);
            unvisited.Remove(current);
        }*/
    }

    /*private Node getNextNode()
    {
        Node candidate = null;
        float cDistance = float.MaxValue;
        foreach (Node n in unvisited)
        {
            if (candidate == null || cDistance > n.estimate)
            {
                candidate = n;
                cDistance = n.estimate;
            }
        }
        return candidate;
    }*/

    void retracePath(Node start, Node end)
    {
        path = new List<Node>();
        Node currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            //Debug.Log("node: " + currentNode.toDebugString());
            currentNode = currentNode.parent;
        }
        path.Reverse();
    }

    float getDistance (Node a, Node b)
    {
        //int dstX = Mathf.Abs(a.gridX - b.gridX);
        //int dstY = Mathf.Abs(a.gridY - b.gridY);

        //if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        //return 14 * dstX + 10 * (dstY - dstX);
        return (a.worldPosition - b.worldPosition).magnitude;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Findpath(transform.position, target);
	}

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            if (grid.grid != null && path != null) {
                foreach (Node n in path)
                {
                    //Debug.Log("ok");
                    Gizmos.color = color;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (grid.nodeRadius * 2 - .1f));                  
                }
            }
        }
    }
}
