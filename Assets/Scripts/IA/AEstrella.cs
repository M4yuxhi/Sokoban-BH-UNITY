using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEstrella : MonoBehaviour
{
    public LevelNode? root;
    public List<KeyValuePair <LevelNode,int>> Open;
    public List<KeyValuePair<LevelNode, int>> Close;


    public AEstrella(LevelNode root) 
    {
        this.root = root;
        Open = new List<KeyValuePair<LevelNode, int>>();
        Close = new List<KeyValuePair<LevelNode, int>>();
        Open.Add(new KeyValuePair<LevelNode,int>(this.root,0));
    }


    /**
     * Retorna un valor aproximado de que tan cerca está de ganar un nivel
     * Entre menor el valor, mejor nodo es
     * 
     **/public int Heuristica(LevelNode node)
    {
        int h = 1000000000;
        List<Vector2> boxes;
        List<Vector2> goals;
        for (int i = 0; i<node.matrixValues.GetLength(0); i++) 
        {
            for (int j = 0; j < node.matrixValues.GetLength(1); j++)
            {
                if (node.matrixValues[i,j]== ) 
                {

                }
            }
        }
        return h;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
