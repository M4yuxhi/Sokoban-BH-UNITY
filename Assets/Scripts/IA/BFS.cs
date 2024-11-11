﻿using System.Collections.Generic;
using UnityEngine;

public static class BFS
{
    public static LevelNode? Algorithm(List<LevelNode> graph, LevelNode start, LevelNode goal)
    {
        Queue<LevelNode> queue = new Queue<LevelNode>();
        HashSet<LevelNode> reached = new HashSet<LevelNode>();

        queue.Enqueue(start);
        reached.Add(start);

        while (queue.Count > 0) 
        { 
            LevelNode t = queue.Dequeue();

            if (t.Solved()) return t;

            foreach (var u in t.GetNeighbors())
            {
                if (!reached.Contains(u))
                {
                    queue.Enqueue(u);
                    reached.Add(u);
                }
            }
        }

        return null;
    }
}