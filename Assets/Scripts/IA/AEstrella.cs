using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEstrella : MonoBehaviour
{
    public LevelNode? root;
    public List<KeyValuePair<LevelNode, int>> Open;
    public List<KeyValuePair<LevelNode, int>> Close;


    public AEstrella(LevelNode root)
    {
        this.root = root;
        Open = new List<KeyValuePair<LevelNode, int>>();
        Close = new List<KeyValuePair<LevelNode, int>>();
        Open.Add(new KeyValuePair<LevelNode, int>(this.root, 0));
    }


    /**
     * Retorna un valor aproximado de que tan cerca está de ganar un nivel
     * Entre menor el valor, mejor nodo es
     * 
     **/
    public int Heuristica(LevelNode node)
    {
        int h = 1000000000;
        List<Vector2> boxes = new List<Vector2>();
        List<Vector2> goals = new List<Vector2>();
        for (int i = 0; i < node.matrixValues.GetLength(0); i++)
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
        if (goals.Count == 0) return 0; //ganamos!
        int nFact = boxes.Count;
        int n = boxes.Count;
        while (n > 1)
        {
            n--;
            nFact *= n;
        }
        Vector2[,] perms = new Vector2[nFact, boxes.Count];

        return h;
    }

    public int Factorial(int n)
    {
        int nFact = n;
        int i = n;
        while (i > 1)
        {
            i--;
            nFact *= i;
        }
        return nFact;
    }

    public void Testing(List<Vector2> vectorList) 
    {
        // Calcula el número de permutaciones (factorial de la cantidad de elementos)
        int numPermutaciones = Factorial(vectorList.Count);
        Vector2[,] matrizPermutaciones = new Vector2[numPermutaciones, vectorList.Count];

        // Llenar la matriz con todas las permutaciones
        List<Vector2[]> listaPermutaciones = new List<Vector2[]>();
        GenerarPermutaciones(vectorList, 0, vectorList.Count - 1, listaPermutaciones);

        // Rellenar la matriz con las permutaciones
        for (int i = 0; i < listaPermutaciones.Count; i++)
        {
            for (int j = 0; j < vectorList.Count; j++)
            {
                matrizPermutaciones[i, j] = listaPermutaciones[i][j];
            }
        }

        // Imprimir la matriz para verificar el resultado
        for (int i = 0; i < numPermutaciones; i++)
        {
            for (int j = 0; j < vectorList.Count; j++)
            {
                Debug.Log($"("+matrizPermutaciones[i, j].x + ","+ matrizPermutaciones[i, j].y +")");
            }
            Debug.Log("");
        }
    }

    // Método recursivo para generar permutaciones
    static void GenerarPermutaciones(List<Vector2> vectorList, int inicio, int fin, List<Vector2[]> resultado)
    {
        if (inicio == fin)
        {
            resultado.Add(vectorList.ToArray());
        }
        else
        {
            for (int i = inicio; i <= fin; i++)
            {
                // Intercambia elementos para crear nuevas permutaciones
                (vectorList[inicio], vectorList[i]) = (vectorList[i], vectorList[inicio]);
                GenerarPermutaciones(vectorList, inicio + 1, fin, resultado);
                (vectorList[inicio], vectorList[i]) = (vectorList[i], vectorList[inicio]); // Deshace el intercambio
            }
        }
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
