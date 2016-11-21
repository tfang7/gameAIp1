using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public float width, height;
    public int row;
    public int col;
    public Vector2 pos;
    public MeshRenderer rend;
    public Material open;
    public Material closed;
    public Material obstacle;
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
    void OnMouseDown()
    {
        Debug.Log("clicked");
    }
    public void Tree()
    {
        state = State.TREE;
        rend.material.color = Color.green;
    }
    public void Obstacle()
    {
        rend.material = obstacle;
        state = State.OBSTACLE;
    }
    public void Path()
    {
        rend.material = closed;
        state = State.PATH;
    }
    public void checking()
    {
        rend.material = open;
    }

    // Update is called once per frame
    void Update () {
        pos = transform.position;
    }
}
