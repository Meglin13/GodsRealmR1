using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ObjectAmountList<T> where T : MonoBehaviour
{
    public List<ObjectAmountElement<T>> list;

    public List<T> GetList()
    {
        var list = new List<T>();

        foreach (var element in this.list)
            list.AddRange(element.GetItems());

        return list;
    }

    public void AddItem(T item, int amount) => list.Add(new(item, amount));
}

[Serializable]
public class ObjectAmountElement<T> where T : MonoBehaviour
{
    public T item;
    public int amount = 1;

    public ObjectAmountElement(T item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public List<T> GetItems()
    {
        var list = new List<T>();

        for (int i = 0; i < amount; i++)
            list.Add(item);

        return list;
    }
}
