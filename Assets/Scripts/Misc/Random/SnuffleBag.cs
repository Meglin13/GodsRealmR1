using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class SnuffleBag<T>
{
    [SerializeField]
    private List<T> items = new List<T>();
    public int Size => items.Count;
    private Random random = new Random();
    private T currentItem;
    private int currentPosition = -1;

    public SnuffleBag(List<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }

        Snuffle();
    }

    public SnuffleBag(List<T> items, Random random) : this(items)
    {
        this.random = random;
    }

    public SnuffleBag(Random random)
    {
        this.random = random;
    }

    public void Snuffle()
    {
        int i = items.Count;
        while (i > 1)
        {
            i--;
            int k = random.Next(i + 1);
            (items[i], items[k]) = (items[k], items[i]);
        }
    }

    public void Add(T item, int amount)
    {
        for (int i = 0; i < amount; i++)
            items.Add(item);

        currentPosition = Size - 1;
    }

    public void Add(T item)
    {
        items.Add(item);
        currentPosition = Size - 1;
    }

    public T Next()
    {
        if (currentPosition < 1)
        {
            currentPosition = Size - 1;
            currentItem = items[0];
            return currentItem;
        }
        var pos = random.Next(currentPosition);
        currentItem = items[pos];
        items[pos] = items[currentPosition];
        items[currentPosition] = currentItem;
        currentPosition--;
        return currentItem;
    }
}
