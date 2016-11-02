using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Astar : MonoBehaviour {
    public List<Node> neighbors;
    public List<string> closedListNames;
    public float hn, gn;
    public Tile start, dest;
    public BoardGenerator graph;
    public GameObject test;
    public Dictionary<int, Node[]> path;
    public Node startNode, endNode, currentNode;
    public bool running;
    public bool reset, done;
    public GameObject startText, endText;
    public int x1, y1, x2, y2;
    public int counter = 0;
    public bool waypointsEnabled, trigger;
    public Node[] pathSelection;
    public List<AstarInstance> exploredPaths;
    public List<Node> propagation;
    public GameObject pathInstance;
    public List<AstarInstance> astarPaths;
    public int propCounter = 0;
    /*
    In A*, evaluation function f(n) = g(n) + h(n)
    g(n) = cost so far to reach n
    h(n) = estimated cost from n to the goal
    f(n) = estimated total cost of path through n to the goal
    */
    // Use this for initialization
    void Start () {
        propagation = new List<Node>();
        exploredPaths = new List<AstarInstance> ();
        astarPaths = new List<AstarInstance>();
        done = false;
        reset = false;
        running = false;
        waypointsEnabled = false;
        trigger = false;
        graph = GameObject.FindGameObjectWithTag("board").GetComponent<BoardGenerator>();


        start = graph.board[106, 240];
        dest = graph.board[112, 256];
        pathSelection = new Node[2];
        pathSelection[0] = start.GetComponentInParent<Node>();
        pathSelection[1] = dest.GetComponentInParent<Node>();
        startNode = pathSelection[0];
        GameObject p = (GameObject)Instantiate(pathInstance, transform);
        AstarInstance a = p.GetComponent<AstarInstance>();
        init(a, pathSelection[0], pathSelection[1]);
        updateDest(a, pathSelection[0], pathSelection[1]); 
        //getNeighbors(currentNode);
    }
    void init(AstarInstance a, Node start, Node end)
    {

        List<Node>  openList = new List<Node>();
        a.open = openList;

        List<Node>  closedList = new List<Node>();
        a.closed = closedList;

        a.start = start; a.end = end;
        //  a.start.tag = "start"; a.end.tag = "end";
        //Debug.Log(astarPaths.Count);
        if (!astarPaths.Contains(a)) astarPaths.Add(a);



    }
    void clear(AstarInstance a)
    {

        foreach (Node n in a.closed)
        {
            foreach (Tile t in n.tile)
            {
                if (t != null)
                {
                    t.Path();
                }
            }
        }
        init(astarPaths[0], pathSelection[0], pathSelection[1]);
        reset = false;
        running = false;
        done = false;
    }
   
    void fnNode(AstarInstance a, Node current, Vector3 start, Vector3 end)
    {
        List<Node> closedList = a.closed;
        List<Node> openList = a.open;
        end = a.end.pos; start = a.start.pos;

        if (!done)
        {
            counter++;
            foreach (Tile t in current.tile)
            {
                if (t != null)
                {
                    t.checking();
                }
            }
            if (!closedList.Contains(current))
            {
                openList.Remove(current);
                closedList.Add(current);
            }
            if (current.tag == a.end.tag)
            {
                done = true;
            }
            else
            {
                neighbors = getNeighbors(current);

                current.setCost(distanceTo(current.pos, start), distanceTo(current.pos, end));

                Node n = getClosestNode(current, neighbors, start, end);
                Debug.DrawLine(n.pos, current.pos, Color.blue, 100f);
                if (closedList.Contains(n) && distanceTo(current.pos, end) > distanceTo(n.pos, end))
                {
                    n.transform.parent = current.transform;
                    n.setCost(distanceTo(n.pos, start), distanceTo(n.pos, end));
                    current = n;
                }
                else if (openList.Contains(n) && distanceTo(current.pos, end) > distanceTo(n.pos, end))
                {
                    n.transform.parent = currentNode.transform;
                    n.setCost(distanceTo(n.pos, start), distanceTo(n.pos, end));
                    current = n;
                }
                else
                {
                    n.setCost(distanceTo(n.pos, start), distanceTo(n.pos, end));
                    if (!openList.Contains(n)) openList.Add(n);
                }
            }
            if (counter >= 1500)
            {
                Debug.Log("A* timed out, no available path");
                done = true;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        endNode = pathSelection[0];
        if (astarPaths.Count > 0)
        {
            findPath(astarPaths[0]);
        }

    }
    void findPath(AstarInstance a)
    {
        List<Node> openList = a.open;
        List<Node> closedList = a.closed;
        if (graph.generated)
        {
            Node s = pathSelection[0];
            Node e = pathSelection[1];
            if (!running)
            {
                init(a, a.start, a.end);
                openList.Add(a.start);
                currentNode = a.start;
                running = true;
            }
           
            if (running)
            {
                updateDest(a, a.start, a.end);
                if (openList.Count > 0)
                {
                    fnNode(a, openList[0], a.start.pos, a.end.pos);
                }
                else
                {
                    clear(astarPaths[0]);
                    if (!done)
                    {
                   //     propagation = getNeighbors(a.start);
                        exploredPaths.Add(a);
                        astarPaths.Remove(a);
                        clear(a);
                        a.closed = new List<Node>();
                        a = astarPaths[0];
                        //propCounter += 1;
                        //if (propCounter >= propagation.Count)
                        //{
                        //    propCounter = 0;
                        //}
                        //a.start = propagation[propCounter];
                        updateDest(a, a.start, a.end);
                        running = false;
                    }


                }

                if (reset) clear(a);
            }
        }
        Debug.DrawLine(a.start.pos, a.end.pos, Color.red, 100f);

    }
    void updateDest(AstarInstance a, Node start, Node end)
    {
        a.start = start;// start.transform.parent.GetComponent<Node>();
        a.end = end;// dest.transform.parent.GetComponent<Node>();
        a.start.tag = "start";
        a.end.tag = "end";
    }
    List<Node> getNeighbors(Node tile)
    {
        //octile movement
        List<Node> neighborsList = new List<Node>();
        Debug.Log("grabbing neighbors");
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
        if (parent.tile[0] != null)
        {
            x = parent.tile[0].col;
            y = parent.tile[0].row;
            int dx = 0;
            int dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = -1;
            dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = -1;
            dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
        }
        if (parent.tile[1] != null)
        {
            x = parent.tile[1].col;
            y = parent.tile[1].row;
            int dx = 0;
            int dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = -1;
            dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = -1;
            dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);

        }
        if (parent.tile[2] != null)
        {
            x = parent.tile[2].col;
            y = parent.tile[2].row;
            int dx = 1;
            int dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = 1;
            dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = 0;
            dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
        }
        if (parent.tile[3] != null)
        {
            x = parent.tile[3].col;
            y = parent.tile[3].row;
            int dx = 1;
            int dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = 1;
            dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
            dx = 0;
            dy = 1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighborsList);
        }
        return neighborsList;
    }
    void checkTile(Tile[,] board, int x, int y, int dx, int dy, Node parent, List<Node> list)
    {
        int r = y + dy;
        int c = x + dx;
       // Debug.Log("r:" + r + ", " + "c:" + c);
        if (r >= 0 && r <= graph.height && c >= 0 && c <= graph.width)
        {
            Node node = board[r, c].GetComponentInParent<Node>();
            bool inList = list.Contains(node);
            if (board[r, c].state == Tile.State.PATH && !inList )
            {
                bool copy = false;
                foreach (AstarInstance path in astarPaths)
                {
                    Debug.Log(path.start == node);
                    if (path.start == node)
                    {
                        copy = true;
                    }
                }
                if (!copy)
                {
                    GameObject path = (GameObject)Instantiate(pathInstance, node.transform);
                    path.transform.parent = transform;
                    AstarInstance a = path.GetComponent<AstarInstance>();
                    init(a, node, endNode);
                }

                list.Add(board[r, c].GetComponentInParent<Node>());
                node.transform.parent = test.transform;
            }
          //  Debug.Log(neighbors.Count);
        }
    }
    float distanceTo(Vector3 a, Vector3 b)
    {
        //return Vector3.Distance(a, b);
        //Manhattan Distance
        Vector3 aPos = a;
        Vector3 bPos = b;
        return Mathf.Sqrt(Mathf.Pow(aPos.x - bPos.x, 2) + (Mathf.Pow(aPos.y - bPos.y,2)));
    }
    public Node getClosestNode(Node current, List<Node> neighbors, Vector3 start, Vector3 end)
    {
        Node closest = neighbors[0];
        float currentGoalDist = distanceTo(current.pos, end);
        float low = distanceTo(neighbors[0].pos, end);
        foreach (Node n in neighbors)
        {
            float checkDist = distanceTo(n.pos, end);
            if (checkDist < low)
            {
                low = checkDist;
                closest = n;
                closest.setCost(distanceTo(closest.pos, start), distanceTo(closest.pos, end));
            }
        }
        return closest;
    }
    public bool checkBlocked(Node n)
    {
        int counter = 0;
        foreach (Tile t in n.tile)
        {
            if (t != null)
            {
                counter++;
            }
        }
        if (counter == 4) return true;
        return false;
    }
}
