using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
public static class A_Star
{
    public static List<Vector2> BuscarRuta(LevelNode raiz)
    {
        var abiertos = new SortedSet<LevelNode>(); // Abiertos, ordenados por f = costo + heurística
        var cerrados = new HashSet<LevelNode>(); // Conjunto de nodos ya visitados
        var ruta = new List<Vector2>();

        abiertos.Add(raiz);

        while (abiertos.Count > 0)
        {
            // Obtener el nodo con el menor f
            LevelNode actual = abiertos.Min;
            abiertos.Remove(actual);

            if (actual.Solved())
            {
                // Construir la ruta desde la meta hacia el nodo raíz
                while (actual.parent != null)
                {
                    ruta.Add(actual.dirFromParent);
                    actual = actual.parent;
                }
                ruta.Reverse(); // Invertir para que quede desde la raíz hasta la meta
                return ruta;
            }

            cerrados.Add(actual);

            // Generar los hijos del nodo actual
            foreach (LevelNode hijo in actual.GetNeighbors())
            {
                if (cerrados.Contains(hijo))
                    continue; // Saltar si ya fue visitado

                if (abiertos.Contains(hijo))
                {
                    // Actualizar el coste si encontramos un camino mejor
                    LevelNode nodoExistente = abiertos.TryGetValue(hijo, out nodoExistente) ? nodoExistente : null;
                    if (nodoExistente != null && hijo.cost < nodoExistente.cost)
                    {
                        abiertos.Remove(nodoExistente);
                        abiertos.Add(hijo);
                    }
                }
                else
                {
                    abiertos.Add(hijo);
                }
            }
        }

        return null; // No se encontró una ruta
    }

    


    /*public static bool Algorithm(List<LevelNode> graph, LevelNode start, LevelNode goal)
    {
        var queue = new PriorityQueue<LevelNode>();
        HashSet<LevelNode> reached = new HashSet<LevelNode>();
        Dictionary<LevelNode, int> dist = new Dictionary<LevelNode, int>
        {
            { start, 0 }
        };
        queue.Enqueue(start, f(start, dist, goal));

        while (queue.Count > 0)
        {
            var t = queue.Dequeue();

            if (t.Equals(goal))
                return true;

            reached.Add(t);

            foreach (var neighbor in t.GetNeighbors())
            {
                int newDist = dist[t] + GetCost(t, neighbor);

                if (!dist.ContainsKey(neighbor) || newDist < dist[neighbor])
                {
                    dist[neighbor] = newDist;
                    int priority = f(neighbor, dist, goal);
                    queue.Enqueue(neighbor, priority);
                }
            }
        }

        return false;
    }

    private static int f(LevelNode node, Dictionary<LevelNode, int> dist, LevelNode goal)
    {
        return dist[node] + Heuristic(node, goal);
    }

    private static int GetCost(LevelNode begin, LevelNode target)
    {
        return 1;
    }

    private static int Heuristic(LevelNode current, LevelNode target)
    {
        return 0;
    }
    */
}