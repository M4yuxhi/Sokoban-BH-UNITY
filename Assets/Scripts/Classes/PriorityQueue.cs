using System;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private SortedDictionary<int, Queue<T>> dictionary = new SortedDictionary<int, Queue<T>>();

    public int Count => dictionary.Count;

    public void Enqueue(T item, int priority)
    {
        if (!dictionary.ContainsKey(priority))
            dictionary[priority] = new Queue<T>();

        dictionary[priority].Enqueue(item);
    }

    public T Dequeue()
    {
        if (dictionary.Count == 0) throw new InvalidOperationException("The queue is empty");

        var firstKey = default(int);

        foreach (var key in dictionary.Keys)
        {
            firstKey = key;
            break;
        }

        var item = dictionary[firstKey].Dequeue();

        if (dictionary[firstKey].Count == 0)
            dictionary.Remove(firstKey);

        return item;
    }

    public T Dequeue(int priority)
    {
        if (dictionary.ContainsKey(priority) && dictionary[priority].Count > 0)
        {
            var item = dictionary[priority].Dequeue();

            // Eliminar la entrada si ya no hay elementos en ese nivel de prioridad
            if (dictionary[priority].Count == 0)
                dictionary.Remove(priority);

            return item;
        }
        else throw new InvalidOperationException($"No items found at priority level {priority}");
    }

    public bool IsEmpty => dictionary.Count == 0;
}