﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Node : MonoBehaviour {
    public Vector3 pos;
    public GameObject center;
    public List<Tile> children;
    public Tile[] tile = new Tile[4];
    public float heuristic;
    public float cost;
    public float costFromStart;

    public BoardGenerator board;
    public float weight;
    // Use this for initialization
    void Start () {
        weight = 0f;
        board = GameObject.FindGameObjectWithTag("board").GetComponent<BoardGenerator>();
        transform.name = board.walkable.Count.ToString();
        Tile[] tiles = GetComponentsInChildren<Tile>();
        if (transform.childCount > 0)
        {
            drawCenter(tiles);
        }

    }
    void drawCenter(Tile[] tiles)
    {
        Vector3 positions = Vector3.zero;
        foreach (Tile t in tiles)
        {
            positions += new Vector3(t.gameObject.transform.position.x, t.gameObject.transform.position.y, 0f);
        }
        GameObject c = Instantiate(center);
        Vector2 actualCenter = (positions / transform.childCount);
        c.transform.parent = transform;
        pos = new Vector3(actualCenter.x, actualCenter.y, -1f);
        c.transform.position = pos;
        board.walkable.Add(this);

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
}