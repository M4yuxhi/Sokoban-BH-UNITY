using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelNode
{
    public short[,] matrixValues;
    public LevelNode? parent;

    public LevelNode(Level level)
    {
        Debug.Log("Size x:" + ((int)level.Size.x) + ",  Size y:" + ((int)level.Size.y));
        Debug.Log("player x:" + ((int)level.PlayerInitialPos.x) + ",  player y:" + ((int)level.PlayerInitialPos.y));
        matrixValues = new short[(1+(int)level.Size.x), (1+(int)level.Size.y)];
        Debug.Log("Size of matrix: " + matrixValues.GetLength(0) + "," + (matrixValues.GetLength(1)));
        for (int i = 0; i < (int)level.Size.x; i++ ) 
        { 
            for (int j = 0; j < (int)level.Size.y; j++)
            {
                matrixValues[i,j] = 0;  //initialice empty, as floor as default
            } 
        }
        matrixValues[(int)level.PlayerInitialPos.x, (int)level.PlayerInitialPos.y] = 2;
        for (int index = 0; index < level.WallsPos.Count; index++)
        {
            try
            {
                matrixValues[(int)level.WallsPos[index].x, (int)level.WallsPos[index].y] = 1;
                //Debug.Log("Success ");
                //Debug.Log("Success at wall: "+ (int)level.WallsPos[index].x+ "," + (int)level.WallsPos[index].y);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                //Debug.Log("Error Happened at index [" + index + "]:   Wall x:" + ((int)level.WallsPos[index].x) + ",  Wall y:" + ((int)level.WallsPos[index].y));
                continue;
            }            
        }

        
        foreach (Vector2 pos in level.GoalsPos)
        {
            matrixValues[(int)pos.x, (int)pos.y] = 4; //goal
        }
        foreach (Vector2 pos in level.BoxesInitialPos)
        {            
            if (matrixValues[(int)pos.x, (int)pos.y]== 4) //if is already a goal
            {
                matrixValues[(int)pos.x, (int)pos.y] = 5;
            }
            else
            {
                matrixValues[(int)pos.x, (int)pos.y] = 3;
            }
        }
        
        parent = null;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Debugging() 
    {

        string line = "Level Node: \n";
        for (int i = 0; i < matrixValues.GetLength(1); i++)
        {
            for (int j = 0; j < matrixValues.GetLength(0); j++)
            {
                line += (" "+matrixValues[j,i]);
            }
            line += "\n";            
        }
        Debug.Log(line);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
