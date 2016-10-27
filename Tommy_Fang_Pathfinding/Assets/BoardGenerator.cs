using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class BoardGenerator : MonoBehaviour {
    public int xUnit, yUnit;
    public int width, height;
    public GameObject path, tree, obstacle;
    public GameObject NodePrefab;
    public Tile[,] board;
    public List<Node> walkable;
    public GameObject obstacles;
    public GameObject center;
    public string type;
    private void loadFile(string fileName, List<string[]> fileContent)
    {
       
        try
        {
            string line;
            StreamReader reader = new StreamReader(fileName, Encoding.Default);
            using (reader)
            {
                int lineCount = 0;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        fileContent.Add(line.Split(' '));
                        lineCount += 1;
                    }
                    
                }
                while (line != null);
                reader.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("{0}\n" + e.Message);
        }
    }
    
	// Use this for initialization
	void Start () {
        xUnit = 2;
        yUnit = 2;
        parseFile();
        Tileizer();

    }
    void Tileizer()
    {
        GameObject par;
        for (int x = 0; x < height-1; x+=2)
        {
            for (int y = 0; y < width-1; y+=2)
            {
                par = (GameObject)Instantiate(NodePrefab);
                Node c = par.GetComponent<Node>();
                c.tile = new Tile[4];
                if (board[x,y].state == Tile.State.PATH)
                {
                    setParentNode(x, y, par.transform);
                    c.tile[0] = board[x, y];
                }
                if (board[x + 1, y].state == Tile.State.PATH)
                {
                    setParentNode(x + 1, y, par.transform);
                    c.tile[1] = board[x+1, y];
                }
                if (board[x, y + 1].state == Tile.State.PATH)
                {
                    setParentNode(x, y + 1, par.transform);
                    c.tile[2] = board[x, y+1];
                }
                if (board[x + 1, y + 1].state == Tile.State.PATH)
                {
                    setParentNode(x + 1, y + 1, par.transform);
                    c.tile[3] = board[x+1, y+1];
                }
                if (par.transform.childCount == 0) Destroy(par);

            }
        }
    }
    void setParentNode(int x, int y, Transform targetParent)
    {
        if (board[x, y] != null)
        {
            if (board[x, y].state == Tile.State.PATH)
                if (!board[x, y].transform.parent.CompareTag("tiled"))
                {
                    board[x, y].transform.SetParent(targetParent.transform);
                }
        }
    }
    void parseFile()
    {
        List<string[]> fileText = new List<string[]>();
        loadFile("assets/text/arena2.map", fileText);
        int lineCount = 0;
        // fileText.Reverse();
        foreach (string[] line in fileText)
        {
            if (lineCount == 0)
            {
                type = line[1];
            }
            if (lineCount == 1)
            {
                height = int.Parse(line[1]);
            }
            if (lineCount == 2)
            {
                width = int.Parse(line[1]);
            }
            if (lineCount == 3)
            {
            }
            else
            {
                if (lineCount > 3)
                {
                    if (board == null) board = new Tile[height, width];
                    int rowNum = lineCount - 4;
                    int colNum = 0;
                    foreach (string i in line)
                    {
                        //Debug.Log(i);
                        colNum = 0;
                        Tile boardTile;
                        foreach (char c in i)
                        {
                            GameObject t = null;
                            if (c == '@') t = Instantiate(obstacle);
                            if (c == '.') t = Instantiate(path);
                            if (c == 'T') t = Instantiate(tree);
                            if (t != null)
                            {
                                boardTile = t.GetComponent<Tile>();
                                boardTile.row = rowNum;
                                boardTile.col = colNum;
                                boardTile.transform.parent = transform;
                                board[rowNum, colNum] = boardTile;

                            }
                            colNum += 1;
                            if (colNum > width) break;
                        }
                    }
                }
            }

            lineCount += 1;
        }
    }
    // Update is called once per frame
    void Update () {
	   
	}
}
