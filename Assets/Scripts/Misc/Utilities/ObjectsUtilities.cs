using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ObjectsUtilities
{
    public static void PrintAllProterties(object obj)
    {
        if (obj == null)
        {
            Debug.Log("Object is NULL!");
            return;
        }

        string result = "";

        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(obj);
            result += $"{name} = {value}";
        }

        Debug.Log(result);
    }

    public static void UnsubscribeEvents(Action[] delList)
    {
        for (int i = 0; i < delList.Length; i++)
        {
            var actList = delList[i].GetInvocationList();
            foreach (var act in actList)
            {
                delList[i] -= act as Action;
            }
        }
    }

    public static void UnsubscribeEvents(Action del)
    {
        var actList = del.GetInvocationList();
        foreach (var act in actList)
        {
            del -= act as Action;
        }
    }
}
