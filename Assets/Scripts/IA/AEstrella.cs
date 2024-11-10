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
        List<Vector2> boxes = new List<Vector2>();
        List<Vector2> goals = new List<Vector2>();
        for (int i = 0; i<node.matrixValues.GetLength(0); i++) 
        {
            for (int j = 0; j < node.matrixValues.GetLength(1); j++)
            {
                if (node.matrixValues[i, j] == 3)
                    boxes.Add(new Vector2(i, j));
                if (node.matrixValues[i, j] == 4)
                    goals.Add(new Vector2(i, j));
            }
        }
        if (goals.Count != boxes.Count) 
        {
            return h;
            Debug.Log("Error en la Heuristica\nDiferente Nº de cajas que metas");
        }
        int nFact = boxes.Count;
        int i = boxes.Count;
        while (i>1) 
        {
            i--;
            nFact *= i;
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
