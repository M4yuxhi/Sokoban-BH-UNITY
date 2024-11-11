using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class IDA_Star
{
    public static List<Vector2> BuscarRuta(LevelNode raiz)
    {
        List<Vector2> ruta = new List<Vector2>();
        int fLimit = AEstrella.Heuristica(raiz);

        while (true)
        {
            int resultado = BusquedaProfundidadLimitada(raiz, fLimit, ruta);
            if (resultado == 0)
            {
                ruta.Reverse(); // Invertir para que quede desde la raíz hasta la meta
                return ruta;
            }
            if (resultado == int.MaxValue)
            {
                return null; // No se encontró una solución
            }
            fLimit = resultado; // Aumentar el límite para la siguiente iteración
        }
    }

    private static int BusquedaProfundidadLimitada(LevelNode nodo, int fLimit, List<Vector2> ruta)
    {
        int f = nodo.cost + AEstrella.Heuristica(nodo);

        if (f > fLimit)
            return f;

        if (nodo.Solved())
        {
            ruta.Add(nodo.dirFromParent); // Agregar el movimiento hacia este nodo
            return 0;
        }

        int minimoSiguienteLimite = int.MaxValue;

        foreach (LevelNode hijo in nodo.GenerarHijos())
        {
            if (hijo == nodo.Padre) // Evitar retroceder al padre directo
                continue;

            int resultado = BusquedaProfundidadLimitada(hijo, fLimit, ruta);

            if (resultado == 0)
            {
                ruta.Add(hijo.MovimientoDesdePadre); // Agregar el movimiento si estamos en el camino de la meta
                return 0;
            }

            if (resultado < minimoSiguienteLimite)
            {
                minimoSiguienteLimite = resultado;
            }
        }

        return minimoSiguienteLimite;
    }
    /*
    public static int? Algorithm(List<LevelNode> graph, LevelNode start, LevelNode goal)
    {
        int threshold = Heuristic(start, goal); // Establecemos el umbral inicial como la heurística del nodo de inicio

        while (true)
        {
            int result = Search(graph, start, goal, 0, threshold);

            if (result == int.MaxValue) // Si el resultado es infinito, no se encontró un camino
                return null;

            if (result == -1) // Si el resultado es -1, el objetivo ha sido encontrado
                return threshold;

            threshold = result; // Actualizamos el umbral para la siguiente iteración
        }
    }

    // Búsqueda DFS limitada por el umbral
    private static int Search(List<LevelNode> graph, LevelNode current, LevelNode goal, int gCost, int threshold)
    {
        int f = gCost + Heuristic(current, goal); // Calculamos f(n) = g(n) + h(n)

        if (f > threshold)
            return f; // Si f supera el umbral, retornamos f(n) para actualizar el umbral

        if (current.Equals(goal))
            return -1; // Si alcanzamos el objetivo, retornamos -1

        int minimum = int.MaxValue; // Almacenamos el valor mínimo de f(n) encontrado

        foreach (var neighbor in current.GetNeighbors())
        {
            int newGCost = gCost + GetCost(current, neighbor);
            int result = Search(graph, neighbor, goal, newGCost, threshold); // Llamada recursiva para cada vecino

            if (result == -1) // Si el objetivo es encontrado
                return -1;

            if (result < minimum) // Actualizamos el mínimo si es necesario
                minimum = result;
        }

        return minimum; // Retornamos el valor más pequeño de f(n) que exceda el umbral
    }

    private static int Heuristic(LevelNode current, LevelNode goal)
    {
        return AEstrella.Heuristica(current);
        //return 0; // Ejemplo de heurística (deberás ajustarlo según el problema)
    }

    private static int GetCost(LevelNode begin, LevelNode target)
    {
        return 1; // Suponiendo un costo constante de 1 por movimiento
    }

    */



}
