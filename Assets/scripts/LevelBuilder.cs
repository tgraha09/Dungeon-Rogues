using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using v3 = UnityEngine.Vector3;
using uRandom = UnityEngine.Random;

using System;
struct Space
{
    public int row;
    public int column;
    public GameObject obj;
    public string tagName;
    public void print(string _note)
    {
        Debug.Log(_note + " Row: " + row + " Column: " + column); 
    }
}
public class LevelBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject normal;
    [SerializeField]
    private GameObject ladder_open;
    [SerializeField]
    private GameObject ladder_closed;
    [SerializeField]
    private GameObject ladder_wall;
    [SerializeField]
    private GameObject end_left;
    [SerializeField]
    private GameObject end_right;
    [SerializeField]
    int columns;
    [SerializeField]
    int rows;
    private Space[,] spaces;
    private List<Space> ladders_closed;
    private List<Space> ladders_open;
    void Start()
    {
       
        GenerateBasic();
        BuildLadders();
        //BuildLadderWalls();
        BuildBoard();
        

    }

    



    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildLadderWalls()
    {
        for (int i = 0; i < ladders_closed.Count; i++)
        {
            int row_diff = 1;
            Space ladder = ladders_closed[i];

            if (Mathf.Abs(ladder.row - row_diff) > 0 && ladder.column + 1 < columns)
            {

                Space open_left = spaces[Mathf.Abs(ladder.row - row_diff), ladder.column + 1];
                if (open_left.tagName == "ladder_open")
                {
                    open_left.print("Left ");
                    open_left.obj = ladder_wall;
                    open_left.tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column + 1].tagName = "ladder_closed";
                    spaces[ladder.row, ladder.column + 1].obj = ladder_closed;
                    spaces[ladder.row, ladder.column].tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column].obj = ladder_wall;
                }

            }
            if (ladder.row + row_diff < rows && ladder.column + 1 < columns)
            {
                Space open_right = spaces[ladder.row + row_diff, ladder.column + 1];
               
                if (open_right.tagName == "ladder_open")
                {
                    open_right.print("Right ");
                    open_right.obj = ladder_wall;
                    open_right.tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column + 1].tagName = "ladder_closed";
                    spaces[ladder.row, ladder.column + 1].obj = ladder_closed;

                    spaces[ladder.row, ladder.column].tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column].obj = ladder_wall;
                }
            }
        }
    }

    private void BuildLadders()
    {
        Space previous = spaces[0, 0];
        Space previous_second = spaces[0, 0];
        ladders_closed = new List<Space>();
        ladders_open = new List<Space>();
        for (int c = 0; c < columns-1; c++)
        {
            int new_row = 0;
            var diff = 0;
            var diff_second = 0;
            try
            {
                
                if(c < 2)
                {
                    do
                    {
                        new_row = uRandom.Range(1, rows - 1);
                        diff = Mathf.Abs(previous.row - new_row);

                    } while (previous.row == new_row || diff < 4);
                    
                }
                else
                {

                    
                    /*

                    //bool check = c > 2 && previous_second.tagName == "ladder_open";
                    previous_second = spaces[previous.row, previous.column-1];
                    diff_second = Mathf.Abs(previous_second.row - new_row);
                    previous.print("1st");
                    previous_second.print("2nd");
                    if (diff_second < 2)
                    {
                        //previous.print("1st");
                        //previous_second.print("2nd");
                    }*/
                    //Space left
                    /*do
                    {
                        new_row = uRandom.Range(1, rows - 1);
                        diff = Mathf.Abs(previous.row - new_row);
                        diff_second = Mathf.Abs(previous_second.row - new_row);

                    } while ((previous.row == new_row || diff < 4) 
                    && (previous_second.row == new_row || diff_second < 4)); //&& check
                    previous_second.print("");*/

                }

                Space s = spaces[new_row, c];
                s.obj = ladder_open;
                s.row = new_row;
                s.column = c;
                s.tagName = "ladder_open";
                s.obj.transform.position = new v3(s.row, s.column, 0);
                spaces[new_row, c] = s;
                ladders_open.Add(s);
                
                previous_second = previous;
                previous = s;
                previous.print("1st");
                previous_second.print("2nd");


            }
            catch (Exception error)
            {
                Debug.Log(error.Message);
            }
        }


        /*for (int i = 0; i < ladders_open.Count; i++)
        {
            Space ladder = ladders_open[i];
            Space closed = spaces[ladder.row, ladder.column + 1];
            closed.obj = ladder_closed;
            closed.tagName = "ladder_closed";
            spaces[closed.row, closed.column] = closed;
            ladders_closed.Add(closed);
        }*/

    }

  

    private void BuildBoard()
    {

        for (int r = 0; r < spaces.GetLength(0); r++)
        {
            for (int c = 0; c < spaces.GetLength(1); c++)
            {
               Space s = spaces[r,c];
               s.obj.transform.position = new v3(s.row, s.column, 0);
               Instantiate(s.obj, s.obj.transform.position, Quaternion.Euler(0f, 180f, 0f));
            }
        }
        
    }

    private void GenerateBasic()
    {
        spaces = new Space[rows, columns];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Space s = new Space();
                s.row = r;
                s.column = c;
                if (r == 0)
                {
                    s.obj = end_left;
                    spaces[r, c] = s;
                    spaces[r, c].tagName = "end_left";
                }
                else if (r == rows - 1)
                {
                    s.obj = end_right;
                    spaces[r, c] = s;
                    spaces[r, c].tagName = "end_right";
                }
                else
                {
                    s.obj = normal;
                    spaces[r, c] = s;
                    spaces[r, c].tagName = "normal";
                }
            }
        }
    }
}
