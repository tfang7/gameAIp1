using UnityEngine;
using System.Collections;

public class mouse : MonoBehaviour {
    public Astar pathfinding;
    public Node[] route;
    public CircleCollider2D col;
	// Use this for initialization
	void Start () {
        col = GetComponent<CircleCollider2D>();
        route = new Node[2];
        pathfinding = GameObject.Find("A*").GetComponent<Astar>();
       // checkVision();

    }

    void OnMouseOver()
    {
        //Debug.Log("clicked");
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
            Debug.Log("m click");
            route[1] = transform.parent.GetComponent<Node>();
            pathfinding.pathSelection[1] = route[1];
        }
    }
    void checkVision()
    {
     //   Debug.DrawLine(new Vector3(0f,0f,0f), transform.position, Color.red, 100f);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, col.radius);
        foreach (Collider2D c in cols)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (c.transform.position - transform.position));
            if (hit.rigidbody != null)
            {
                Debug.DrawLine(hit.rigidbody.transform.position, transform.position, Color.green, 100f);
            }
            if (hit.collider != null)
            {
                if (transform.parent.name != hit.collider.transform.parent.name)
                {
                  //  Debug.Log("not null");
                    Debug.Log("current wp:" + transform.parent.name + " hit name" + hit.collider.transform.parent.name);
                 //   Debug.DrawLine(hit.collider.transform.position, transform.position, Color.green, 100f);

                }
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
