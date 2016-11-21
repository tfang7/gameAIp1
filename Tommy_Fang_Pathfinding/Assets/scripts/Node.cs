using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Node : MonoBehaviour {
    public Vector3 pos;
    public GameObject center, waypointcenter;
    public List<Tile> children;
    public Tile[] tile = new Tile[4];
    public float heuristic;
    public float cost;
    public float costFromStart;
    public BoardGenerator board;
    public bool blocked;
    // Use this for initialization
    void Start () {
        blocked = false;
        board = GameObject.FindGameObjectWithTag("board").GetComponent<BoardGenerator>();
        Tile[] tiles = GetComponentsInChildren<Tile>();
        if (transform.childCount > 0)
        {
            drawCenter(tiles);
        }

    }

    void drawCenter(Tile[] tiles)
    {
        Vector3 positions = Vector3.zero;
        int blocked = tiles.Length;
        foreach (Tile t in tiles)
        {
            if (t.state == Tile.State.OBSTACLE || t.state == Tile.State.TREE) blocked--;
            positions += new Vector3(t.gameObject.transform.position.x, t.gameObject.transform.position.y, 0f);
        }
        GameObject c = null;
        if (board.boardState == BoardGenerator.BoardType.TILE) c = Instantiate(center);
        if (blocked > 0)
        {
            if (board.boardState == BoardGenerator.BoardType.WAYPOINT)
            {
                c = Instantiate(waypointcenter);
                c.name = board.walkable.Count.ToString();
            }
            transform.name = board.walkable.Count.ToString();
            board.walkable.Add(this);

        }
        Vector2 actualCenter = (positions / transform.childCount);
        if (c != null)
        {
            c.transform.parent = transform;
            pos = new Vector3(actualCenter.x, actualCenter.y, -1f);
            c.transform.position = pos;
            center = c;
        }
        BoxCollider2D boxCol = this.GetComponent<BoxCollider2D>();
        if (boxCol == null)
        {
            BoxCollider2D bc = gameObject.AddComponent<BoxCollider2D>();
            bc.offset = actualCenter;
            bc.size *= 2;
            //  bc.offset = new Vector2(targetParent.transform.position.x, targetParent.transform.position.y);
        }
        else
        {
            boxCol.offset = actualCenter;
            boxCol.size *= 2;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
    public void setCost(float distFromStart, float euclideanCost)
    {
        costFromStart = distFromStart;
        heuristic = euclideanCost;
        cost = costFromStart + heuristic;
    }
    public void setText()
    {

    }
}
