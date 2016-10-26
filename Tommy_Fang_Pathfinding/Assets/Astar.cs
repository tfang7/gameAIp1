using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Astar : MonoBehaviour {
    public List<chunk> openList;
    public List<chunk> closedList;
    public List<chunk> neighbors;
    public float hn, gn;
    public Vector2 start;
    public Vector2 end;
    public bool done;
    public BoardGenerator graph;
    /*
    In A*, evaluation function f(n) = g(n) + h(n)
    g(n) = cost so far to reach n
    h(n) = estimated cost from n to the goal
    f(n) = estimated total cost of path through n to the goal
    */
    // Use this for initialization
    void Start () {
        done = false;
        graph = GameObject.FindGameObjectWithTag("board").GetComponent<BoardGenerator>();
        fn();

	}
    void fn()
    {
        //
        Debug.Log("grabbing neighbors");
        getNeighbors(graph.board[106,240]);
    }
	// Update is called once per frame
	void Update () {
	
	}
    float getCost(Tile start, Tile end)
    {
        return Vector3.Distance(start.transform.position, end.transform.position);
    }
    void getNeighbors(Tile tile)
    {
        //octile movement
        neighbors = new List<chunk>();
        /*+-------+
         *|x-1,y-1|
         *+------+-----+-----+
         *       |x,y  |x+1,y|
         *       +-----+-----+
         *       |x,y+1|   
         *       +-----+
         */
        chunk parent = tile.GetComponentInParent<chunk>();
        //neighbors.Add(parent);
        int x, y;
        Tile[,] tileBoard = graph.board;
        if (parent == null) return;
        Debug.Log(parent.tile.Length);
        foreach (Tile t in parent.tile)
        {
            Debug.Log(t);
        }
        if (parent.tile[0] != null)
        {
            x = parent.tile[0].col;
            y = parent.tile[0].row;
            int dx = 0;
            int dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
            dx = -1;
            dy = -1;
            checkTile(tileBoard, x, y, dx, dy, parent,  neighbors);
            dx = -1;
            dy = 0;
            checkTile(tileBoard, x, y, dx, dy, parent, neighbors);
        }
        //if (parent.tile[1] != null)
        //{
        //    x = parent.tile[1].col;
        //    y = parent.tile[1].row;
        //    int dx = 0;
        //    int dy = 1;
        //    checkTile(tileBoard, x, y, dx, dy, neighbors);
        //    dx = -1;
        //    dy = 1;
        //    checkTile(tileBoard, x, y, dx, dy, neighbors);
        //    dx = -1;
        //    dy = 0;
        //    checkTile(tileBoard, x, y, dx, dy, neighbors);
        //}
        //if (parent.tile[2] != null)
        //{

        //}
        //if (parent.tile[3] != null)
        //{

        //}
    }
    void checkTile(Tile[,] board, int x, int y, int dx, int dy, chunk parent, List<chunk> neighbors)
    {
        int r = y + dy;
        int c = x + dx;
        Debug.Log("r:" + r + ", " + "c:" + c);
        if (r >= 0 && r <= graph.height && c >= 0 && c <= graph.width)
        {
            bool inList = neighbors.Contains(board[r, c].GetComponentInParent<chunk>());
            if (board[r, c].state == Tile.State.PATH && !inList )
            {
                neighbors.Add(board[r, c].GetComponentInParent<chunk>());
            }
            Debug.Log(neighbors.Count);
        }

    }
}
