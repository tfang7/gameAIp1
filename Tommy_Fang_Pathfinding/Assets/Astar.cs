using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Astar : MonoBehaviour {
    public List<chunk> openList;
    public List<chunk> closedList;
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

	}
    void fn()
    {

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
        List<chunk> neighbors = new List<chunk>();
        /*+-------+
         *|x-1,y-1|
         *+------+-----+-----+
         *       |x,y  |x+1,y|
         *       +-----+-----+
         *       |x,y+1|   
         *       +-----+
         */
        chunk parent = tile.GetComponentInParent<chunk>();
        int x, y;
        Tile[,] tileBoard = graph.board;
        if (parent.tile[0] != null)
        {
            x = parent.tile[0].col;
            y = parent.tile[0].row;
        }
        if (parent.tile[1] != null)
        {

        }
        if (parent.tile[2] != null)
        {

        }
        if (parent.tile[3] != null)
        {

        }
    }
    void checkTile(Tile[,] board, int x, int y, int dir, List<chunk> neighbors)
    {
        int r = x + (1 * dir);
        int c = y + (1 * dir);
        if (r >= 0 && r <= graph.width)
        {
            bool inList = neighbors.Contains(board[r, y].GetComponentInParent<chunk>());
            if (board[r, y].state == Tile.State.PATH && !inList)
            {
                neighbors.Add(board[r, y].GetComponentInParent<chunk>());
            }
        }
        if (c >= 0 && c <= graph.height)
        {
            if (board[x,c].state == Tile.State.PATH)
            {

            }
        }


    }
}
