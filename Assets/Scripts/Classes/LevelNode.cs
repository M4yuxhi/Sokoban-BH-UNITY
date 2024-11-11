using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelNode
{
    //private LevelNode[] neighbors = new LevelNode[4];

    public short[,] matrixValues;
    public LevelNode? parent;
    public static int ClassID;
    public int thisId;

    public static Action<LevelNode> UpdateScreen;
    public int cost;
    public Vector2 dirFromParent;

    public LevelNode(Level level)
    {
        //COmo llamar a UpdateScreen? ez
        //UpdateScreen?.Invoke(this);
        cost = 0;
        matrixValues = new short[(int)level.Size.x, (int)level.Size.y];
        for (int i = 0; i < (int)level.Size.x; i++)
        {
            for (int j = 0; j < (int)level.Size.y; j++)
            {
                matrixValues[i, j] = 0;  //inicialice vacIo, como piso en caso default
            }
        }
        ClassID = 0;
        thisId = 0;
        foreach (Vector2 pos in level.WallsPos)
        {
            matrixValues[(int)pos.x, (int)level.Size.y - (int)pos.y ] = 1; //wall
        }
        foreach (Vector2 pos in level.GoalsPos)
        {
            matrixValues[(int)pos.x, (int)level.Size.y - (int)pos.y] = 4; //goal
        }
        if (matrixValues[(int)level.PlayerInitialPos.x, (int)level.Size.y - (int)level.PlayerInitialPos.y] == 4)
        {
            matrixValues[(int)level.PlayerInitialPos.x, (int)level.Size.y - (int)level.PlayerInitialPos.y] = 6;
        }
        if (matrixValues[(int)level.PlayerInitialPos.x, (int)level.Size.y - (int)level.PlayerInitialPos.y] == 0)
        {
            matrixValues[(int)level.PlayerInitialPos.x, (int)level.Size.y - (int)level.PlayerInitialPos.y] = 2;
        }
        foreach (Vector2 pos in level.BoxesInitialPos)
        {
            if (matrixValues[(int)pos.x, (int)level.Size.y - (int)pos.y] == 4) //if is already a goal
            {
                matrixValues[(int)pos.x, (int)level.Size.y - (int)pos.y] = 5; //Box in goal
            }
            else
            {
                matrixValues[(int)pos.x, (int)level.Size.y - (int)pos.y] = 3; //Box
            }
        }
        parent = null;
    }

    public bool EqualsById(LevelNode other) 
    {
        return (thisId ==other.thisId);
    }

    public bool EqualsButDifferentParent(LevelNode anotherNode)
    {
        if (this.matrixValues.GetLength(0) != anotherNode.matrixValues.GetLength(0) || this.matrixValues.GetLength(1) != anotherNode.matrixValues.GetLength(1))
        {
            return false;
        }
        if (anotherNode.parent.EqualsById(parent) ) { return false; }
        for (int i = 0; i < matrixValues.GetLength(0); i++)
        {
            for (int j = 0; j < matrixValues.GetLength(1); j++)
            {
                if (matrixValues[i, j] != anotherNode.matrixValues[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }


    /**
     * Compara 2 LevelNode para saber si son idEnticos o no 
     * Sirve para en la bUsqueda no agregar nodos idEnticos o no agregar nodos de otras dimensiones
     * Si tienen diferentes dimensiones: false
     * Si todos sus valores son iguales: false
     * Si al menos un valor es distinto: true
     *
     *
     **/
    public bool isNewValidNode(LevelNode anotherNode)
    {
        if (this.matrixValues.GetLength(0)!= anotherNode.matrixValues.GetLength(0) || this.matrixValues.GetLength(1) != anotherNode.matrixValues.GetLength(1)) 
        {
            return false;
        }

        for (int i = 0; i<matrixValues.GetLength(0); i++) 
        {
            for (int j = 0; j < matrixValues.GetLength(1); j++)
            {
                if (matrixValues[i,j]!=anotherNode.matrixValues[i,j] || !parent.Equals(anotherNode.parent)) 
                {
                    return true;
                }
            }
        }
        return false;
    }

    public LevelNode CreateChild(Vector2 direction)
    {
        float largo = Mathf.Sqrt((direction.x * direction.x) + (direction.y * direction.y));
        if ((direction.x != 0 && direction.x != 1 && direction.x != -1) || (direction.y != 0 && direction.y != 1 && direction.y != -1) || largo != 1)
        {
            return null;
        }
        else
        {
            if (ValidateChild(direction))
            {
                return new LevelNode(this, direction);
            }
            else
            {
                return null;
            }
        }
    }

    /**
     * MEtodo que valida si el nivel estA resuelto o no
     * True: Ganaste
     * False: Siga participando, por que debe continuar la bUsqueda
     *
     */public bool Solved() 
    {
        short boxesInGoal = 0;
        short boxesNotGoal = 0;
        for (short i = 0; i < matrixValues.GetLength(0); i++)
        {
            for (short j = 0; j < matrixValues.GetLength(1); j++)
            {
                if (matrixValues[i,j]==3) { boxesNotGoal++; }
                if (matrixValues[i, j] == 5) { boxesInGoal++; }
            }
        }return (boxesInGoal > 0 && boxesNotGoal == 0); 
    }

    /**
     * Valida que el movimiento sea legal
     * True: Movimiento vAlido
     * False: No se debe crear ese nodo
     *
     */
    public bool ValidateChild(Vector2 direction)
    {
        short playerX = -1;
        short playerY = -1;
        for (short i = 0; i < matrixValues.GetLength(0); i++)
        {
            for (short j = 0; j < matrixValues.GetLength(1); j++)
            {
                if (matrixValues[i, j] == 2 || matrixValues[i, j] == 6)
                {
                    playerX = i;
                    playerY = j;
                }
            }
        }
        if (playerY == -1 || playerX == -1) //no hay jugador
        {
            return false;
        }
        switch (direction.x)
        {
            case 1:
                if (playerX + 1 >= matrixValues.GetLength(0)) { return false; }//Me salgo del mapa                
                if (matrixValues[(playerX + 1), playerY] == 1) { return false; } //Muro
                if ((matrixValues[(playerX + 1), playerY] == 3 || matrixValues[(playerX + 1), playerY] == 5) && playerX + 2 >= matrixValues.GetLength(1)) { return false; }//caja luego se sale
                if ((matrixValues[(playerX + 1), playerY] == 3 || matrixValues[(playerX + 1), playerY] == 5) && (matrixValues[(playerX + 2), playerY] == 3 || matrixValues[(playerX + 2), playerY] == 5 || matrixValues[(playerX + 2), playerY] == 1 ))//caja y luego caja o Muro, o me salgo
                {
                    return false;
                }
                break;
            case -1:
                if (playerX - 1 < 0) { return false; }//Me salgo del mapa                
                if (matrixValues[(playerX - 1), playerY] == 1) { return false; } //Muro
                if ((matrixValues[(playerX - 1), playerY] == 3 || matrixValues[(playerX - 1), playerY] == 5) && playerX - 2 <0) { return false; }//caja luego se sale
                if ((matrixValues[(playerX - 1), playerY] == 3 || matrixValues[(playerX - 1), playerY] == 5) && (matrixValues[(playerX - 2), playerY] == 3 || matrixValues[(playerX - 2), playerY] == 5 || matrixValues[(playerX - 2), playerY] == 1))//caja y luego caja o Muro, o me salgo
                {
                    return false;
                }
                break;
        }
        switch (direction.y)
        {
            case 1:
                if (playerY + 1 >= matrixValues.GetLength(1)) { return false; }//Me salgo del mapa                
                if (matrixValues[playerX, (playerY + 1)] == 1) { return false; } //Muro
                if ((matrixValues[playerX, (playerY + 1)] == 3 || matrixValues[playerX, (playerY + 1)] == 5) && playerY + 2 >= matrixValues.GetLength(1)) { return false; }//caja luego se sale
                if ((matrixValues[playerX, (playerY + 1)] == 3 || matrixValues[playerX, (playerY + 1)] == 5) && (matrixValues[playerX, (playerY + 2)] == 3 || matrixValues[playerX, (playerY + 2)] == 5 || matrixValues[playerX, (playerY + 2)] == 1))//caja y luego caja o Muro, 
                {
                    return false;
                }
                break;
            case -1:
                if (playerY - 1 < 0) { return false; }//Me salgo del mapa                
                if (matrixValues[playerX, (playerY - 1)] == 1) { return false; } //Muro
                if ((matrixValues[playerX, (playerY - 1)] == 3 || matrixValues[playerX, (playerY - 1)] == 5) && playerY - 2 < 0) { return false; }//caja luego se sale
                if ((matrixValues[playerX, (playerY - 1)] == 3 || matrixValues[playerX, (playerY - 1)] == 5) && (matrixValues[playerX, (playerY - 2)] == 3 || matrixValues[playerX, (playerY - 2)] == 5 || matrixValues[playerX, (playerY - 2)] == 1 ))//caja y luego caja o Muro
                {
                    return false;
                }
                break;
        }
        return true;
    }

    public LevelNode(LevelNode parent, Vector2 direction)
    {
        matrixValues = new short[parent.matrixValues.GetLength(0), parent.matrixValues.GetLength(1)];
        this.parent = parent;
        short playerX = -1;
        short playerY = -1;
        for (short i = 0; i < matrixValues.GetLength(0); i++)
        {
            for (short j = 0; j < matrixValues.GetLength(1); j++)
            {
                this.matrixValues[i, j ] = parent.matrixValues[i, j];
                if (parent.matrixValues[i, j ] == 2 || parent.matrixValues[i, j] == 6) //hay un jugador
                {
                    switch (parent.matrixValues[i, j])
                    {
                        case 2: matrixValues[i, j] = 0; break;
                        case 6: matrixValues[i, j] = 4; break;
                        default: break;
                    }
                playerX = 
                playerY = j;
                }
            }
            
        }
        switch (direction.x)
        {
            case 1:
                if (matrixValues[playerX + 1, playerY] == 3 || matrixValues[playerX + 1, playerY] == 5) //hay caja
                {
                    if (matrixValues[playerX + 2, playerY] == 0){matrixValues[playerX + 2, playerY] = 3;}
                    if (matrixValues[playerX + 2, playerY] == 4){matrixValues[playerX + 2, playerY] = 5;}
                    if (matrixValues[playerX + 1, playerY] == 3) { matrixValues[playerX + 1, playerY] = 2; }
                    if (matrixValues[playerX + 1, playerY] == 5) { matrixValues[playerX + 1, playerY] = 6; }
                }
                if (matrixValues[playerX + 1, playerY] == 0) { matrixValues[playerX + 1, playerY] = 2; }
                if (matrixValues[playerX + 1, playerY] == 4) { matrixValues[playerX + 1, playerY] = 6; }

                break;
            case -1:
                if (matrixValues[playerX - 1, playerY] == 3 || matrixValues[playerX + 1, playerY] == 5) //hay caja
                {
                    if (matrixValues[playerX - 2, playerY] == 0) { matrixValues[playerX - 2, playerY] = 3; }
                    if (matrixValues[playerX - 2, playerY] == 4) { matrixValues[playerX - 2, playerY] = 5; }
                    if (matrixValues[playerX - 1, playerY] == 3) { matrixValues[playerX - 1, playerY] = 2; }
                    if (matrixValues[playerX - 1, playerY] == 5) { matrixValues[playerX - 1, playerY] = 6; }
                }
                if (matrixValues[playerX - 1, playerY] == 0) { matrixValues[playerX - 1, playerY] = 2; }
                if (matrixValues[playerX - 1, playerY] == 4) { matrixValues[playerX - 1, playerY] = 6; }
                break;
            default: break;
        }
        switch (direction.y)
        {
            case 1:
                if (matrixValues[playerX , playerY + 1] == 3 || matrixValues[playerX , playerY + 1] == 5) //hay caja
                {
                    if (matrixValues[playerX , playerY + 2] == 0) { matrixValues[playerX, playerY + 2] = 3; }
                    if (matrixValues[playerX , playerY + 2] == 4) { matrixValues[playerX, playerY + 2] = 5; }
                    if (matrixValues[playerX , playerY + 1] == 3) { matrixValues[playerX, playerY + 1] = 2; }
                    if (matrixValues[playerX , playerY + 1] == 5) { matrixValues[playerX, playerY +1] = 6; }
                }
                if (matrixValues[playerX, playerY + 1] == 0) { matrixValues[playerX, playerY + 1] = 2; }
                if (matrixValues[playerX, playerY + 1] == 4) { matrixValues[playerX, playerY + 1] = 6; }

                break;
            case -1:
                if (matrixValues[playerX, playerY - 1] == 3 || matrixValues[playerX, playerY-1] == 5) //hay caja
                {
                    if (matrixValues[playerX, playerY - 2] == 0) { matrixValues[playerX, playerY - 2] = 3; }
                    if (matrixValues[playerX, playerY - 2] == 4) { matrixValues[playerX, playerY - 2] = 5; }
                    if (matrixValues[playerX, playerY - 1] == 3) { matrixValues[playerX, playerY - 1] = 2; }
                    if (matrixValues[playerX, playerY - 1] == 5) { matrixValues[playerX, playerY - 1] = 6; }
                }
                if (matrixValues[playerX, playerY - 1] == 0) { matrixValues[playerX, playerY - 1] = 2; }
                if (matrixValues[playerX, playerY - 1] == 4) { matrixValues[playerX, playerY - 1] = 6; }
                break;
            default: break;
        }
        dirFromParent = direction;
        cost = parent.cost + 1;
        ClassID++;
        thisId = ClassID;
    }

    public List<LevelNode> GetNeighbors()
    {
        List<LevelNode> neighbors = new List<LevelNode>();
        List<Vector2> dirs = new List<Vector2>();
        dirs.Add(new Vector2(1, 0));
        dirs.Add(new Vector2(0, 1));
        dirs.Add(new Vector2(-1, 0));
        dirs.Add(new Vector2(0, -1));
        for (int i =0;i<dirs.Count;i++) 
        {
            if (!dirs[i].Equals(-this.dirFromParent))
            {
                if (ValidateChild(dirs[i])) 
                { 
                    LevelNode neighbor = new LevelNode(this, dirs[i]);
                    neighbors.Add(neighbor);
                }
            }
        }
        return neighbors;
    }

    public void Debugging() 
    {
        string line = "Level Node:  "+this.thisId+"\n";
        for (int i = 0; i < matrixValues.GetLength(1); i++)
        {
            for (int j = 0; j < matrixValues.GetLength(0); j++)
            {
                line += ("  |  "+matrixValues[j,i] );
            }
            line += "  |\n";
            line += "====================================================\n";
        }
        Debug.Log(line);
        List<Vector2> path = A_Star.BuscarRuta(this);
        if (path != null)
        {
            Debug.Log("IDA:\n" + path);
        }
        else
        {
            Debug.Log("Camino no hallado:\n" );
        }
        //AEstrella.Heuristica(this);
    }
    
}
