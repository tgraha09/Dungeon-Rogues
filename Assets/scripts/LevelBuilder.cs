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
    public GameObject prefab;
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
    [SerializeField]
    int ladder_difference;
    private Space[,] spaces;
    private List<GameObject> all_spaces;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            DeleteBoard();
            GenerateBasic();
            BuildLadders();
            //BuildLadderWalls();
            BuildBoard();
        }
    }

    private void DeleteBoard()
    {
        foreach (var space in all_spaces)
        {
            Destroy(space);
        }
        

    }

    private void BuildLadderWalls()
    {
        for (int i = 0; i < ladders_closed.Count; i++)
        {
            int row_diff = 1;
            Space ladder = ladders_closed[i];

            /*if (Mathf.Abs(ladder.row - row_diff) > 0 && ladder.column + 1 < columns)
            {

                Space open_left = spaces[Mathf.Abs(ladder.row - row_diff), ladder.column + 1];
                if (open_left.tagName == "ladder_open")
                {
                    open_left.print("Left ");
                    open_left.prefab = ladder_wall;
                    open_left.tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column + 1].tagName = "ladder_closed";
                    spaces[ladder.row, ladder.column + 1].prefab = ladder_closed;
                    spaces[ladder.row, ladder.column].tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column].prefab = ladder_wall;
                }

            }*/
            /*if (ladder.row + row_diff < rows && ladder.column + 1 < columns)
            {
                Space open_right = spaces[ladder.row + row_diff, ladder.column + 1];
               
                if (open_right.tagName == "ladder_open")
                {
                    open_right.print("Right ");
                    open_right.prefab = ladder_wall;
                    open_right.tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column + 1].tagName = "ladder_closed";
                    spaces[ladder.row, ladder.column + 1].prefab = ladder_closed;

                    spaces[ladder.row, ladder.column].tagName = "ladder_wall";
                    spaces[ladder.row, ladder.column].prefab = ladder_wall;
                }
            }*/
        }
    }

    private void BuildLadders()
    {
        Space previous_ladder = spaces[0, 0];
        ladders_open = new List<Space>();
        ladders_closed = new List<Space>();
        var level1 = 1;
        var level2 = 0;
        var diff = ladder_difference;
        for (int r = 0; r < rows-1; r++)
        {
            int new_column = 0;
            var diff_lvl1 = 0;
            var diff_lvl2 = 0;
            
            try
            {
                //previous.print("");
                if (r < 2)
                {
                    do
                    {
                        new_column = uRandom.Range(1, columns - 1);
                        diff_lvl1 = Mathf.Abs(previous_ladder.column - new_column);

                    } while (previous_ladder.column == new_column || diff_lvl1 < diff);

                }
                else
                {
                    do
                    {
                        new_column = uRandom.Range(1, columns - 1);
                        diff_lvl1 = Mathf.Abs(ladders_open[level1].column - new_column);
                        diff_lvl2 = Mathf.Abs(ladders_open[level2].column - new_column);

                    } while ((ladders_open[level1].column == new_column || diff_lvl1 < diff) || (ladders_open[level2].column == new_column || diff_lvl2 < diff)); //
                    level1++;
                    level2++;
                }

                Space s = spaces[new_column, r];
                s.prefab = ladder_open;
                s.column = new_column;
                s.row = r;
                s.tagName = "ladder_open";
                s.prefab.transform.position = new v3(s.column, s.row, 0);
                spaces[new_column, r] = s;
                ladders_open.Add(s);

                Space closed = spaces[new_column, r+1];
                closed.prefab = ladder_closed;
                closed.column = new_column;
                closed.row = r+1;
                closed.tagName = "ladder_closed";
                closed.prefab.transform.position = new v3(s.column, s.row+1, 0);
                spaces[new_column, r+1] = closed;
                ladders_closed.Add(closed);

                previous_ladder = s;
                

            }
            catch (Exception error)
            {
                Debug.Log(error.Message);
            }
            
        }
    }

    


    private void BuildBoard()
    {
        all_spaces = new List<GameObject>();
        for (int c = 0; c < spaces.GetLength(0); c++) 
        {
            for (int r = 0; r < spaces.GetLength(1); r++)
            {
               Space s = spaces[c, r];
               s.prefab.transform.position = new v3(s.column, s.row, 0);
               GameObject gobj = Instantiate(s.prefab, s.prefab.transform.position, Quaternion.Euler(0f, 180f, 0f));
               all_spaces.Add(gobj);
            }
        }
        
    }

    private void GenerateBasic()
    {
        spaces = new Space[columns, rows];
        for (int c = 0; c < columns; c++) 
        {
            for (int r = 0; r < rows; r++)
            {
                Space s = new Space();
                s.row = r;
                s.column = c;
                if (c == 0)
                {
                    s.prefab = end_left;
                    spaces[c, r] = s;
                    spaces[c, r].tagName = "end_left";
                }
                else if (c == columns - 1)
                {
                    s.prefab = end_right;
                    spaces[c, r] = s;
                    spaces[c, r].tagName = "end_right";
                }
                else
                {
                    s.prefab = normal;
                    spaces[c, r] = s;
                    spaces[c, r].tagName = "normal";
                }
            }
        }
    }
}
