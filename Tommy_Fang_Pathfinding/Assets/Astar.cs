using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Astar : MonoBehaviour {
    public List<Node> openList;
    public List<Node> closedList;
    public List<string> closedListNames;
    public List<Node> neighbors;
    public float hn, gn;
    public Tile start;
    public Tile dest;
    public bool done;
    public BoardGenerator graph;
    public GameObject test;
    public Dictionary<int, Node[]> path;
    public Node startNode;
    public Node endNode;
    public Node currentNode;
    /*
    In A*, evaluation function f(n) = g(n) + h(n)
    g(n) = cost so far to reach n
    h(n) = estimated cost from n to the goal
    f(n) = estimated total cost of path through n to the goal
    */
    // Use this for initialization
    void Start () {
        done = false;
        closedListNames = new List<string>();
        graph = GameObject.FindGameObjectWithTag("board").GetComponent<BoardGenerator>();
        openList = new List<Node>();
        start = graph.board[106, 240];
        dest = graph.board[110, 100];

        startNode = start.GetComponentInParent<Node>();
        endNode = dest.GetComponentInParent<Node>();
        startNode.setCost(0, distanceTo(startNode, endNode));

        openList.Add(startNode);
        currentNode = startNode;
        fn(start, dest);

    }
  
    void fn(Tile start, Tile end)
    {
        
        while (!done)
        {
            if (startNode == endNode)
            {
                Debug.Log("finished");
                done = true;
            }
            else
            {
                getNeighbors(currentNode);
                foreach (Node n in neighbors)
                {
                    if (closedList.Contains(n) && currentNode.cost < n.cost)
                    {
                        n.transform.parent = currentNode.transform;
                        n.setCost(distanceTo(n, startNode), distanceTo(n, endNode));
                        currentNode = n;
                    }
                    else if (openList.Contains(n) && currentNode.cost < n.cost)
                    {
                        n.transform.parent = currentNode.transform;
                        n.setCost(distanceTo(n, startNode), distanceTo(n, endNode));
                        currentNode = n;
                    }
                    else
                    {
                        openList.Add(n);
                        //setCost(float distFromStart, float euclideanCost)
                        n.setCost(distanceTo(n, startNode), distanceTo(n, endNode));
                    }
                }
                done = true;
            }

        }
    }
    // Update is called once per frame
    void Update () {
        Debug.DrawLine(startNode.pos, endNode.pos, Color.red, 100f);
    }
    float getCost(Tile start, Tile end)
    {
        return Vector3.Distance(start.transform.position, end.transform.position);
    }
    void getNeighbors(Node tile)
    {
        //octile movement
        neighbors = new List<Node>();
        /*+-------+
         *|x-1,y+1|
         *+------+-----+-----+
         *       |x,y  |x+1,y|
         *       +-----+-----+
         *       |x,y-1|   
         *       +-----+
         */
        Node parent = tile;//.GetComponentInParent<Node>();
        int x, y;
        Tile[,] tileBoard = graph.board;
        if (parent == null) return;
        if (parent.tile[0] != null)
        {
            x = parent.tile[0].col;
            y = parent.tile[0].row;
            int dx = 0;
            int dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = -1;
            dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = -1;
            dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
        }
        if (parent.tile[1] != null)
        {
            x = parent.tile[1].col;
            y = parent.tile[1].row;
            int dx = 0;
            int dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = -1;
            dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = -1;
            dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);

        }
        if (parent.tile[2] != null)
        {
            x = parent.tile[2].col;
            y = parent.tile[2].row;
            int dx = 1;
            int dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = 1;
            dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = 0;
            dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
        }
        if (parent.tile[3] != null)
        {
            x = parent.tile[3].col;
            y = parent.tile[3].row;
            int dx = 1;
            int dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = 1;
            dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = 0;
            dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
        }
    }
    void checkTile(Tile[,] board, int x, int y, int dx, int dy, Node parent, List<Node> neighbors)
    {
        int r = y + dy;
        int c = x + dx;
       // Debug.Log("r:" + r + ", " + "c:" + c);
        if (r >= 0 && r <= graph.height && c >= 0 && c <= graph.width)
        {
            Node node = board[r, c].GetComponentInParent<Node>();
            bool inList = neighbors.Contains(node);
            if (board[r, c].state == Tile.State.PATH && !inList )
            {
                neighbors.Add(board[r, c].GetComponentInParent<Node>());
                node.transform.parent = test.transform;
            }
          //  Debug.Log(neighbors.Count);
        }
    }
    float distanceTo(Node a, Node b)
    {
        //Manhattan Distance
        Vector3 aPos = a.pos;
        Vector3 bPos = b.pos;
        return Mathf.Pow(Mathf.Sqrt(aPos.x - bPos.x), 2) +
               Mathf.Pow(Mathf.Sqrt(aPos.y - bPos.y), 2);
    }
}
