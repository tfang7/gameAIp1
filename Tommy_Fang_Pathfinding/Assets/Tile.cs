using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public float width, height;
    public int row;
    public int col;
    public Vector2 pos;
    public MeshRenderer rend;
    public enum State {
        TREE,
        OBSTACLE,
        PATH
    };
    public State state;
	// Use this for initialization
	void Start () {
        transform.position = new Vector2(col * 1, row * 1);
       
        rend = GetComponent<MeshRenderer>();
    }
    public void Tree()
    {
        rend = GetComponent<MeshRenderer>();
        state = State.TREE;
        rend.material.color = Color.green;
    }
    public void Obstacle()
    {
        rend.material.color = Color.black;
        state = State.OBSTACLE;
    }
    public void Path()
    {
        rend.material.color = Color.white;
        state = State.PATH;
    }
    // Update is called once per frame
    void Update () {
        pos = transform.position;
    }
}
