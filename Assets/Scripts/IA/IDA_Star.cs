using System;
using System.Collections.Generic;

public static class IDA_Star
{
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
        return 0; // Ejemplo de heurística (deberás ajustarlo según el problema)
    }

    private static int GetCost(LevelNode begin, LevelNode target)
    {
        return 1; // Suponiendo un costo constante de 1 por movimiento
    }
}
