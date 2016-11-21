using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class mouse : MonoBehaviour {
    public Astar pathfinding;
    public Node[] route;
    public CircleCollider2D col;
    public List<GameObject> waypointNeighbors;
	// Use this for initialization
	void Start () {
        waypointNeighbors = new List<GameObject>();
        col = GetComponent<CircleCollider2D>();
        route = new Node[2];
        pathfinding = GameObject.Find("A*").GetComponent<Astar>();
        if (pathfinding.waypointsEnabled)
        {
            checkVision();
        }
        //Debug.Log("i'm alive");

    }
    //controls for toggling obstacle states of tiles
    void OnMouseOver()
    {
        //Debug.Log("clicked");
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && Input.GetMouseButtonDown(0))
        {
            //if CTRL + Mouse
            Tile[] tiles = transform.parent.GetComponent<Node>().tile;
            foreach (Tile t in tiles)
            {
                Debug.Log(t.state);
                if (t.state == Tile.State.PATH)
                {
                    t.Obstacle();
                    Debug.Log(t.state);
                }
                else if (t.state == Tile.State.OBSTACLE || t.state == Tile.State.TREE)
                {
                    t.Path();
                }
            }
            Debug.Log("ctrl + mouse clicked");
        }
        else
        {
            //path setting, set start pos on left click, destination on right click
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("left click");
                route[0] = transform.parent.GetComponent<Node>();
                pathfinding.pathSelection[0] = route[0];
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("right click");
                route[1] = transform.parent.GetComponent<Node>();
                pathfinding.pathSelection[1] = route[1];
            }
            if (Input.GetMouseButtonDown(2))
            {
                // Debug.Log("m click");
                route[1] = transform.parent.GetComponent<Node>();
                pathfinding.pathSelection[1] = route[1];
            }

        }

    }
    //checks if waypoints are in radius of collider and raycasts
    public void checkVision()
    {
     //   Debug.DrawLine(new Vector3(0f,0f,0f), transform.position, Color.red, 100f);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D c in cols)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (c.transform.position - transform.position));
            BoxCollider2D box = c.GetComponent<BoxCollider2D>();
            if (box != null)
            {
                Vector3 offset = new Vector3(box.offset.x, box.offset.y, -1f);
             //   Debug.DrawLine(offset, transform.position, Color.red, 50f);
                hit = Physics2D.Raycast(transform.position, (offset - transform.position));
            }
            if (hit.collider != null && c.gameObject != this.gameObject)
            {
                //  Debug.Log("not null");
                //   Debug.Log("current wp:" + transform.parent.name + " hit name" + hit.collider.transform.parent.name);
                if (box == null)
                {
                    if (hit.collider.CompareTag("waypoint"))
                    {
                        if (!waypointNeighbors.Contains(c.gameObject) && Vector3.Distance(transform.position, c.gameObject.transform.position) < 7.5f)
                        {
                            Debug.DrawLine(c.transform.position, transform.position, Color.green, 50f);
                            waypointNeighbors.Add(c.gameObject);
                        }
                    }
                    else
                    {
                     //   Debug.DrawLine(hit.collider.transform.position, transform.position, Color.blue, 50f);
                    }
                    

                }


            }
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        //
        Debug.Log(coll.gameObject.name);
       // Debug.DrawLine(coll.transform.position, transform.position, Color.green, 100f);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
