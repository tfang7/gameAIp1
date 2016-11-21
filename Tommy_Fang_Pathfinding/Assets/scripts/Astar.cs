using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Astar : MonoBehaviour {
    public List<Node> neighbors;
    public Tile start, dest;
    public BoardGenerator graph;
    public GameObject test;
    public Node startNode, endNode, currentNode;
    public bool running;
    public bool reset, done;
    public int x1, y1, x2, y2;
    public int counter = 0;
    public bool waypointsEnabled;
    public Node[] pathSelection;
    public GameObject pathInstance;
    public List<AstarInstance> astarPaths;
    public List<Node> searched;
    public List<Node> searchspace;
    public AstarInstance currentPath;
    bool trigger;
    /*
    In A*, evaluation function f(n) = g(n) + h(n)
    g(n) = cost so far to reach n
    h(n) = estimated cost from n to the goal
    f(n) = estimated total cost of path through n to the goal
    */
    // Use this for initialization
    void Start () {
        trigger = false;
        astarPaths = new List<AstarInstance>();
        done = false;
        reset = false;
        running = false;
        graph = GameObject.FindGameObjectWithTag("board").GetComponent<BoardGenerator>();
        start = graph.board[12, 108];
        dest = graph.board[12, 96];
        waypointCheck();
        pathSelection = new Node[2];
        pathSelection[0] = start.GetComponentInParent<Node>();
        pathSelection[1] = dest.GetComponentInParent<Node>();
        startNode = pathSelection[0];
  
        //getNeighbors(currentNode);
    }
    void waypointCheck()
    {
        if (waypointsEnabled)
        {
            start = graph.board[175, 91];
            dest = graph.board[170, 100];
        }
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
   //A* ALGORITHM using tile representation
    void fnNode(AstarInstance a, Node current, Vector3 start, Vector3 end)
    {
        //Create an a* path list, initializing the closed/open lists
        List<Node> closedList = a.closed;
        List<Node> openList = a.open;
        end = a.end.pos; start = a.start.pos;
        //set start / end board pos target
        if (!done)
        {
            counter++;
            //This colors each tile
            foreach (Tile t in current.tile)
            {
                if (t != null)
                {
                    t.checking();
                }
            }
            //add to closed list if current node is in open list
            if (!closedList.Contains(current))
            {
                openList.Remove(current);
                closedList.Add(current);
            }
            //ending condiition
            if (current.tag == a.end.tag || current.name == a.end.name)
            {
                done = true;
            }
            else
            {
                //get list of adjacent tiles
                neighbors = getNeighbors(current, a);
                //set f(n) = g(n) + h(n), where g(n) = dist to goal, h(n) = dist from start
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
    void findPathWaypoints(AstarInstance a, Node current, Vector3 start, Vector3 end)
    {
        //Create an a* path list, initializing the closed/open lists
        List<Node> closedList = a.closed;
        List<Node> openList = a.open;
        end = a.end.pos; start = a.start.pos;
        //set start / end board pos target
        if (!done)
        {
            counter++;
            //This colors each tile
            foreach (Tile t in current.tile)
            {
                if (t != null)
                {
                    t.checking();
                }
            }
            //add to closed list if current node is in open list
            if (!closedList.Contains(current))
            {
                openList.Remove(current);
                closedList.Add(current);
            }
            //ending condiition
            if (current.tag == a.end.tag)
            {
                done = true;
            }
            else
            {
                //get list of adjacent tiles
                //set f(n) = g(n) + h(n), where g(n) = dist to goal, h(n) = dist from start
                current.setCost(distanceTo(current.pos, start), distanceTo(current.pos, end));

                Node n = getClosest(current, a);
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
            if (counter >= 2000)
            {
                Debug.Log("A* timed out, no available path");
                done = true;
            }
        }
    }
    // Update is called once per frame
    void Update () {

        startNode = pathSelection[0];
        endNode = pathSelection[1];
        if (startNode != null && endNode != null && !trigger)
        {
            GameObject p = (GameObject)Instantiate(pathInstance, transform);
            AstarInstance a = p.GetComponent<AstarInstance>();
            init(a, pathSelection[0], pathSelection[1]);
            updateDest(a, pathSelection[0], pathSelection[1]);
            trigger = true;

        }
        if (astarPaths.Count > 0)
        {
            currentPath = astarPaths[0];
            findPath(currentPath);
        }

    }
    //find path algorithm for tile based A*
    void findPath(AstarInstance a)
    {
        List<Node> openList = a.open;
        List<Node> closedList = a.closed;
        if (graph.generated)
        {

                //Node s = pathSelection[0];
                //Node e = pathSelection[1];
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
                        if (!waypointsEnabled)
                            fnNode(a, openList[0], a.start.pos, a.end.pos);
                        else
                        {
                            findPathWaypoints(a, openList[0], a.start.pos, a.end.pos);
                        }
                    }
                    else
                    {
                        if (done)
                        {
                            searched.Clear();
                            searchspace.Clear();
                        }
                        // astarPaths.Remove(a);
                        // clear(a);
                        if (!done)
                        {
                            if (!searched.Contains(a.start))
                            {
                                searchspace.Remove(a.start);
                                searched.Add(a.start);
                            }
                            clear(a);
                            if (searchspace.Count > 0)
                            {
                                a.start = searchspace[0];
                                searchspace.Remove(searchspace[0]);
                            }
                            updateDest(a, a.start, a.end);
                            running = false;
                        }
                    }
                    if (reset) clear(a);
                }
        }
        Debug.DrawLine(a.start.pos, a.end.pos, Color.red, 100f);

    }
    //update the start and end nodes
    void updateDest(AstarInstance a, Node start, Node end)
    {
        a.start = start;// start.transform.parent.GetComponent<Node>();
        a.end = end;// dest.transform.parent.GetComponent<Node>();
        a.start.tag = "start";
        a.end.tag = "end";
    }
    //get closest waypoint
    Node getClosest(Node tile, AstarInstance a)
    {
        mouse controller = tile.GetComponentInChildren<mouse>();
        if (controller != null) controller.checkVision();
        
        Node closest = controller.waypointNeighbors[0].GetComponentInParent<Node>();
        Debug.Log(controller.waypointNeighbors.Count);
        float low = distanceTo(closest.pos, a.end.pos);
        foreach (GameObject n in controller.waypointNeighbors)
        {
            Node check = n.GetComponentInParent<Node>();
            float checkDist = distanceTo(check.pos, a.end.pos);
            if (checkDist < low)
            {
                low = checkDist;
                closest = check;
                closest.setCost(distanceTo(closest.pos, a.start.pos), distanceTo(closest.pos, a.end.pos));
            }
            else
            {
                check.setCost(distanceTo(check.pos, a.start.pos), distanceTo(check.pos, a.end.pos));
            }
        }
        return closest;
    }
    List<Node> getNeighbors(Node tile, AstarInstance a)
    {
        //octile movement
        List<Node> neighborsList = new List<Node>();
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
        foreach (Node neighbor in neighborsList)
        {
            if (neighbor == a.start)
            {

            }
        }
        return neighborsList;
    }
    void checkTile(Tile[,] board, int x, int y, int dx, int dy, Node parent, List<Node> list)
    {
        int r = y + dy;
        int c = x + dx;
        if (r >= 0 && r <= graph.height && c >= 0 && c <= graph.width)
        {
            Node node = board[r, c].GetComponentInParent<Node>();
            bool inList = list.Contains(node);
            if (board[r, c].state == Tile.State.PATH && !inList )
            {
                //GameObject path = (GameObject)Instantiate(pathInstance, node.transform);
                //path.transform.parent = transform;
                //AstarInstance astar = path.GetComponent<AstarInstance>();
                //init(astar, node, endNode);
                //if (node == endNode || copy)
                //{
                //    Destroy(path);
                //}

                ////if (!pathObjects.Contains(path))
                ////{
                ////    pathObjects.Add(path);
                ////};
                if (!searchspace.Contains(node) && !searched.Contains(node) && 
                    distanceTo(pathSelection[0].pos, pathSelection[1].pos) > distanceTo(node.pos, pathSelection[1].pos))
                    searchspace.Add(node);
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
