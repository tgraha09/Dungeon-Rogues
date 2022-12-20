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
    private GameObject ladder_top;
    [SerializeField]
    private GameObject ladder_bottom;
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
    private List<GameObject> all_GOBJS;
    private List<Space> ladders_closed;
    private List<Space> ladders_open;
    void Start()
    {
        
        GenerateBasic();
        BuildLadders();
        BuildLadderWalls();
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
            BuildLadderWalls();
            BuildBoard();
        }
    }

    private void DeleteBoard()
    {
        foreach (var space in all_GOBJS)
        {
            Destroy(space);
        }
        all_GOBJS.Clear();

    }

    private void BuildLadderWalls()
    {
        for (int i = 0; i < ladders_closed.Count; i++)
        {
            int row_diff = 2;
            Space ladder = ladders_closed[i];

            if (ladder.row + 2 < rows)
            {
                //Debug.Log("IN");
                Space open = spaces[ladder.column,ladder.row+2];
                
                if (open.tagName == "ladder_open")
                {
                    open.print("");

                    open.print("closed");
                    //spaces[open.column, open.row].prefab = ladder_wall;
                    //spaces[open.column, open.row].tagName = "ladder_wall";
                    
                    spaces[ladder.column, ladder.row+1].tagName = "ladder_closed";
                    spaces[ladder.column, ladder.row+1].prefab = ladder_closed;

                    spaces[ladder.column, ladder.row].tagName = "ladder_wall";
                    spaces[ladder.column, ladder.row].prefab = ladder_wall;
                }

            }
        
            
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

    private Space ChangeSpace(int c, int r, string tagName, GameObject ladder, v3 pos)
    {
        Space s = spaces[c, r];
        s.prefab = ladder;
        s.column = c;
        s.row = r;
        s.tagName = tagName;
        s.prefab.transform.position = pos;
        spaces[c, r] = s;
        return s;
    }

    private void BuildBoard()
    {
        all_GOBJS = new List<GameObject>();
        for (int c = 0; c < spaces.GetLength(0); c++) 
        {
            for (int r = 0; r < spaces.GetLength(1); r++)
            {
                Space s = spaces[c, r];
                s.prefab.transform.position = new v3(s.column, s.row, 0);
                GameObject gobj = Instantiate(s.prefab, s.prefab.transform.position, Quaternion.Euler(0f, 180f, 0f));
                all_GOBJS.Add(gobj);
                
                if (s.tagName== "ladder_open")//|| s.tagName == "ladder_closed"
                {
                    v3 pos = s.prefab.transform.position;
                    pos.z = -0.1f;
                    GameObject _ladder = Instantiate(ladder_bottom, pos, Quaternion.Euler(0f, 180f, 0f));
                    all_GOBJS.Add(_ladder);
                }
                else if (s.tagName == "ladder_closed")
                {
                    v3 pos = s.prefab.transform.position;
                    pos.z = -0.1f;
                    GameObject _ladder = Instantiate(ladder_top, pos, Quaternion.Euler(0f, 180f, 0f));
                    all_GOBJS.Add(_ladder);
                }
                else if(s.tagName == "ladder_wall")
                {
                    v3 pos = s.prefab.transform.position;
                    pos.z = -0.1f;
                    GameObject _ladder = Instantiate(ladder_bottom, pos, Quaternion.Euler(0f, 180f, 0f));
                    all_GOBJS.Add(_ladder);
                }
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
