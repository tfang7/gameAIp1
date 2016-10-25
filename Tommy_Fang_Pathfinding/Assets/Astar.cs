using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Astar : MonoBehaviour {
    public List<chunk> openList;
    public List<chunk> closedList;
    public float hn, fn, gn;
    public Vector2 start;
    public Vector2 end;
    public bool done;
    /*
    In A*, evaluation function f(n) = g(n) + h(n)
    g(n) = cost so far to reach n
    h(n) = estimated cost from n to the goal
    f(n) = estimated total cost of path through n to the goal
    */
    // Use this for initialization
    void Start () {
        fn = 0;
        done = false;

	}
	
    void calculateGN()
    {
        
    }
    void calculateFN()
    {

    }
	// Update is called once per frame
	void Update () {
	
	}
}
