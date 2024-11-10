using System.Collections.Generic;

public static class A_Star
{
    public static bool Algorithm(List<LevelNode> graph, LevelNode start, LevelNode goal)
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
}