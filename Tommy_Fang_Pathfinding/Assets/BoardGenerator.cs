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
    public GameObject chunkPrefab;
    public Tile[,] board;
    public List<Tile> walkable;
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
        List<Tile> chunk = new List<Tile>();
        for (int x = 0; x < width-1; x+=2)
        {
            for (int y = 0; y < height-1; y+=2)
            {
                par = (GameObject)Instantiate(chunkPrefab);
                chunk = new List<Tile>();
                if (board[x,y].state == Tile.State.PATH)
                {
                    setParentChunk(x, y, par.transform);
                    chunk.Add(board[x, y]);

                }
                if (board[x + 1, y].state == Tile.State.PATH)
                {
                    setParentChunk(x + 1, y, par.transform);
                    chunk.Add(board[x + 1, y]);
                }
                if (board[x, y + 1].state == Tile.State.PATH)
                {
                    setParentChunk(x, y + 1, par.transform);
                    chunk.Add(board[x, y + 1]);
                }
                if (board[x + 1, y + 1].state == Tile.State.PATH)
                {
                    setParentChunk(x + 1, y + 1, par.transform);
                    chunk.Add(board[x + 1, y + 1]);
                }
                if (par.transform.childCount == 0) Destroy(par);
                else drawCenter(par.transform);

            }
        }
    }
    void setParentChunk(int x, int y, Transform targetParent)
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
    void drawCenter(Transform par)
    {
        Vector3 positions = Vector3.zero;
        foreach (Tile t in par.GetComponentsInChildren<Tile>())
        {
            positions += new Vector3(t.pos.x, t.pos.y, 0f);
        }
        GameObject c = Instantiate(center);
        c.transform.position = positions / par.transform.childCount;

    }
    void parseFile()
    {
        List<string[]> fileText = new List<string[]>();
        loadFile("assets/text/arena2.map", fileText);
        int lineCount = 0;
        // fileText.Reverse();
        foreach (string[] line in fileText)
        {
            Debug.Log(line);
            if (lineCount == 0)
            {
                type = line[1];
            }
            if (lineCount == 1)
            {
                width = int.Parse(line[1]);
            }
            if (lineCount == 2)
            {
                height = int.Parse(line[1]);
            }
            if (lineCount == 3)
            {
            }
            else
            {
                if (lineCount > 3)
                {
                    if (board == null) board = new Tile[width, height];
                    int rowNum = lineCount - 4;
                    int colNum = 0;
                    foreach (string i in line)
                    {
                        //Debug.Log(i);
                        colNum = 0;
                        foreach (char c in i)
                        {
                            if (c == '@')
                            {
                                GameObject t = Instantiate(obstacle);
                                Tile boardTile = t.GetComponent<Tile>();
                                boardTile.row = rowNum;
                                boardTile.col = colNum;
                                boardTile.transform.parent = obstacles.transform;
                                board[rowNum, colNum] = boardTile;

                            }
                            if (c == '.')
                            {
                                GameObject t = Instantiate(path);
                                Tile boardTile = t.GetComponent<Tile>();
                                boardTile.row = rowNum;
                                boardTile.col = colNum;
                                boardTile.transform.parent = transform;
                                board[rowNum, colNum] = boardTile;
                                walkable.Add(boardTile);
                            }
                            if (c == 'T')
                            {
                                GameObject t = Instantiate(tree);
                                Tile boardTile = t.GetComponent<Tile>();
                                boardTile.row = rowNum;
                                boardTile.col = colNum;
                                boardTile.transform.parent = obstacles.transform;
                                board[rowNum, colNum] = boardTile;
                            }
                            colNum += 1;
                            if (colNum > height) break;
                        }
                        //  Debug.Log(lineCount);
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
