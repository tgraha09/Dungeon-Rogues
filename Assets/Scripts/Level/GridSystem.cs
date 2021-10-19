using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using v3 = UnityEngine.Vector3;
public class GridSystem : MonoBehaviour
{

    public class Slot
    {
        public int row;
        public int col;
        public GameObject obj;
        public bool isSpawned;
        public string tag;
        public Color color;
        public Slot(int _r, int _c, GameObject _sp, bool spawn, string t)
        {
            row = _r;
            col = _c;
            obj = _sp;
            isSpawned = spawn;
            tag = t;
            
        }

        public string Tag
        {
            set { tag = value;
                obj.tag = value;
            }
            get { return tag; }
        }

        public void Print()
        {
            Debug.Log("Tag: " + tag + " Row: " + row + " Col: " + col);
        }
    }
    //vox boxes should be 10x10
    public GameObject hall, ladder, floorless, left, right, wall;
    public int ladderPerFloor;
    public Material mat;
    public int rows;
    public int cols;

    private int maxLadderLength;
    private Slot[,] board;
    private List<Slot> ladders;
   // private List<Slot>
    // Start is called before the first frame update
    void Start()
    {
        board = new Slot[rows, cols];
        ladders = new List<Slot>();

        //basic build
        ConstructSpaces();
        GenBaseLadders();
       // CompleteBaseLadders();
        maxLadderLength = cols/2;
        var currLength = maxLadderLength;
        int y_count = 0;
        Slot last_space = null;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var space = board[r, c];
                if (space.Tag == "ladder")
                {
                   /* var possibly = IndexOfBoard(space, 2);
                    Slot last = possibly[0];
                    
                    foreach (var item in possibly)
                    {
                        if (item.tag == "ladder" && last.col != item.col
                            && last.row != item.row)
                        {
                            //found = item;
                            //item.Print();
                        }
                        //last = item;
                    }
                    */
                   
                }

            }
        }

        /*for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var space = board[r, c];
                var count = 0;
                Slot lad = null;
                Slot floor = null;
                for (int k = 0; k < rows; k++)
                {
                    var other = board[k, c];
                    if(other.Tag == "ladder")
                    {
                        lad = other;
                    }
                    if (other.Tag == "floorless")
                    {
                        floor = other;
                    }
                }
                if (lad == null || floor == null && space.Tag != "left" && space.Tag != "right")
                {
                    Debug.Log("ladders missing: ");
                    space.Print();
                }
            }
        }*/

        /**/

        DrawBoard();
    }

    private void GenBaseLadders()
    {
        int y_count = 0;
        Slot last_space = null;
        Slot last_2nd = null;
        Slot first = null;
        //Slot 
        int ranR;
        //Spawn initial base ladders
        for (int i = 0; i < cols; i++)
        {
            if (i == 0) //first ladder at row 0
            {
                //first base ladder pos
                ranR = UnityEngine.Random.Range(1, rows - 1);
                var space = board[ranR, i];
                space.obj = ladder;
                space.Tag = "ladder";
                ladders.Add(space); 
                last_space = space;
                first = space;

            }
            else
            {
                //next base ladder position 
                ranR = UnityEngine.Random.Range(1, rows - 1);
                bool validLadder = false;
                var possible = board[ranR, i];
                //possible.Print();
                while (validLadder == false)
                {
                  
                 
                    if(last_2nd == null)
                    {
                        var rowDiff = Math.Abs(possible.row - last_space.row);
                        bool rowLevel = rowDiff > 2;
                        var rowDiff2 = Math.Abs(possible.row - first.row);
                        bool rowLevel1 = rowDiff2 > 2;
                        if (rowLevel)
                        {
                            validLadder = true;
                        }
                        else
                        {
                            ranR = UnityEngine.Random.Range(1, rows - 1);
                            possible = board[ranR, i];

                        }
                    }
                    else
                    {
                        var rowDiff = Math.Abs(possible.row - last_space.row);
                        bool rowLevel = rowDiff > 2;
                        var rowDiff2 = Math.Abs(possible.row - last_2nd.row);
                        bool rowLevel1 = rowDiff2 > 2;
                        if (rowLevel)
                        {
                            validLadder = true;
                        }
                        else
                        {
                            ranR = UnityEngine.Random.Range(1, rows - 1);
                            possible = board[ranR, i];

                        }
                    }
                          

                }

                var space = possible;
                if (Math.Abs(i - cols) == 1)
                {
                    space.obj = hall;
                    space.Tag = "hall";
                }
                else
                {
                    space.obj = ladder;
                    space.Tag = "ladder";
                    ladders.Add(space);
                }

                if (i >= 2)
                {
                    last_2nd = last_space;
                }
                last_space = space;

            }

        }

        
        //spawn ends to ladders with adjustments
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var space = board[r, c];
                if (c - 1 >= 0 && board[r, c - 1].Tag == "ladder" && space.Tag == "hall"
                    && c + 1 < cols && board[r, c + 1].Tag != "ladder")
                {
                    space.Tag = "floorless";
                    space.obj = floorless;
                }
                else if (c - 1 >= 0 && board[r, c - 1].Tag == "ladder" && space.Tag == "hall"
                    && c + 1 < cols && board[r, c + 1].Tag == "ladder")
                {
                    space.Tag = "wall";
                    space.obj = wall;
                    board[r, c + 1].Tag = "wall";
                    board[r, c + 1].obj = wall;
                }
                else if (cols - c == 1 && board[r, c - 1].Tag == "ladder")
                {
                    board[r, c].Tag = "floorless";
                    board[r, c].obj = floorless;
                    //board[r, c].Print();
                }
                else if (c - 1 >= 0 && board[r, c - 1].Tag == "wall" && space.Tag == "hall")
                {
                    space.Tag = "floorless";
                    space.obj = floorless;
                    //board[r, c].Print();
                }


            }
        }


        //adjusts ends to create openings for hidden passage ways 
        for (int r = 0; r < rows; r++)
         {
             for (int c = 0; c < cols; c++)
             {
                 var space = board[r, c];
                 if (space.Tag == "floorless")
                 {
                     for (int d = 0; d < rows; d++)
                     {
                         var other = board[d, space.col];
                         if (other.Tag == "wall" && space.col + 1 < cols)
                         {
                             space.obj = wall;
                             space.Tag = "wall";
                             board[space.row, space.col + 1].obj = floorless;
                             board[space.row, space.col + 1].tag = "floorless";

                             //other.Print();
                         }


                     }
                 }

             }
         }


    }

    private void CompleteBaseLadders()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var space = board[r, c];
                if (space.Tag == "ladder")
                {
                    if (c + 1 < cols) //&& c-1 >= 0
                    {
                        board[r, c + 1].Tag = "floorless";
                        board[r, c + 1].obj = floorless;
                        // Debug.Log("R: " + (r) + " C: " + (c+1) + " tag: " + board[r, c+1].Tag);
                    }
                    // Debug.Log("R: " + (r) + " C: " + (c + 1) + " tag: " + space.Tag);
                }
                if (space.Tag == "floorless")
                {
                    if (c + 1 < cols && board[r, c + 1].Tag == "ladder") //&& c-1 >= 0
                    {
                        board[r, c].Tag = "wall";
                        board[r, c].obj = wall;
                        board[r, c + 1].Tag = "wall";
                        board[r, c + 1].obj = wall;
                        // Debug.Log("R: " + (r) + " C: " + (c+1) + " tag: " + board[r, c+1].Tag);
                    }

                    // Debug.Log("R: " + (r) + " C: " + (c + 1) + " tag: " + space.Tag);
                }
                if (space.Tag == "wall")
                {
                    space.Print();
                    if (c + 1 < cols && board[r, c + 1].Tag == "hall") //&& c-1 >= 0
                    {
                        //board[r, c].Tag = "wall";
                        //board[r, c].obj = wall;
                        //board[r, c + 1].Tag = "floorless";
                        //board[r, c + 1].obj = floorless;
                        // Debug.Log("R: " + (r) + " C: " + (c+1) + " tag: " + board[r, c+1].Tag);
                    }

                    // Debug.Log("R: " + (r) + " C: " + (c + 1) + " tag: " + space.Tag);
                }
                // Debug.Log("R: " + r + " C: " + c);

            }
        }
    }

    private List<Slot> CheckDowns(Slot space)
    {
        var row = space.row;
        var col = space.col;
        List<Slot> directional = new List<Slot>();
        int dist = 2;
        if (row + dist < rows)
        {
            for (int i = 1; i <= dist; i++)
            {
                var right = board[row + i, col];
                directional.Add(right);
            }

        }
        else if (row + 1 < rows)
        {
            var right = board[row + 1, col];
            directional.Add(right);
        }
        if (row - dist >= 0)
        {
            for (int i = 1; i <= dist; i++)
            {
                var left = board[row - i, col];
                directional.Add(left);
            }

        }
        else if (row - 1 >= 0)
        {
            var left = board[row - 1, col];
            directional.Add(left);
        }
        if (col - dist >= 0)
        {
            for (int i = 1; i <= dist; i++)
            {
                var down = board[row, col - i];
                directional.Add(down);
            }

        }
        else if (col - 1 >= 0)
        {
            var down = board[row, col - 1];
            directional.Add(down);
        }
        

        return directional;
    }

    private List<Slot> IndexOfBoard(Slot space, int dist, string direction)
    {
        var row = space.row;
        var col = space.col;
        List<Slot> directional = new List<Slot>();
        switch (direction)
        {
            case "right":
                if (row + dist < rows)
                {
                    for (int i = 1; i <= dist; i++)
                    {
                        var right = board[row + i, col];
                        directional.Add(right);
                    }

                }
                else if (row + 1 < rows)
                {
                    var right = board[row + 1, col];
                    directional.Add(right);
                }
                break;
            case "left":
                if (row - dist >= 0)
                {
                    for (int i = 1; i <= dist; i++)
                    {
                        var left = board[row - i, col];
                        directional.Add(left);
                    }

                }
                else if (row - 1 >= 0)
                {
                    var left = board[row - 1, col];
                    directional.Add(left);
                }
                break;
            case "up":
                if (col + dist < cols)
                {
                    for (int i = 1; i <= dist; i++)
                    {
                        var up = board[row, col + i];
                        directional.Add(up);
                    }

                }
                else if (col + 1 < cols)
                {
                    var up = board[row, col + 1];
                    directional.Add(up);
                }
                break;
            case "down":
                if (col - dist >= 0)
                {
                    for (int i = 1; i <= dist; i++)
                    {
                        var down = board[row, col - i];
                        directional.Add(down);
                    }

                }
                else if (col - 1 >= 0)
                {
                    var down = board[row, col - 1];
                    directional.Add(down);
                }
                break;
            default:
                break;
        }
        
        
        return directional;
    }

    private void DrawBoard()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var current = board[r, c];
                var pos = new v3(current.row, current.col, 0);
                if(c + 1 < cols)
                {
                    var above = board[r, c+1];
                    if (current.Tag == "ladder" && above.Tag != "floorless")
                    {
                        //Debug.Log("FLOORLESS");
                        //above.obj = wall;
                       // above.Tag = "wall";
                        //GameObject child = above.obj.transform.GetChild(0).gameObject;
                        //SetColor(child, Color.green);
                    }
                    
                }
                else
                {
                    //Ladders = new List<int>(numLadders);
                    
                }
                if(current.Tag == "ladder")
                {
                    //SetColor(space.obj, Color.red);
                }
                Instantiate(current.obj, pos, Quaternion.identity);
                // Debug.Log("R: " + r + " C: " + c);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ConstructSpaces()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                // var pos = new v3(r, c, 0);
                if (r == 0)
                {
                    //Instantiate(left, pos, Quaternion.identity);
                    board[r, c] = new Slot(r, c, left, false, "left");
                    //Spaces.Add(new Slot(r, c, left));
                }
                else if (r == rows - 1)
                {
                    //Instantiate(right, pos, Quaternion.identity);
                    board[r, c] = new Slot(r, c, right, false, "right");
                    //Spaces.Add(new Slot(r, c, right));
                }
                else
                {
                    //Instantiate(hall, pos, Quaternion.identity) ;
                    //Spaces.Add(new Slot(r, c, hall));
                    board[r, c] = new Slot(r, c, hall, false, "hall");
                }
            }
        }
    }
    void SetColor(GameObject obj, Color c)
    {
        var shader = obj.GetComponent<Renderer>();
        //mat.color = Color.white;
        shader.sharedMaterial.color = c;
        //Debug.Log("Shader: " + shader.name);
        //render.material.SetColor("_Color", c);
    }
}
