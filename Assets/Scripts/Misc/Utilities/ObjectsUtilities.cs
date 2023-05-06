using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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
            UnsubscribeEvents(delList[i]);
            delList[i] = null;
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

    public static List<T> FindAllScriptableObjects<T>()
    {
        List<T> tmp = new List<T>();
        string ResourcesPath = Application.dataPath + "/Resources/ScriptableObjects";
        string[] directories = Directory.GetDirectories(ResourcesPath, "*", SearchOption.AllDirectories);

        foreach (string item in directories)
        {
            string itemPath = "ScriptableObjects/" + item.Substring(ResourcesPath.Length + 1);
            T[] reasult = Resources.LoadAll(itemPath, typeof(T)).Cast<T>().ToArray();

            foreach (T x in reasult)
            {
                if (!tmp.Contains(x))
                    tmp.Add(x);
            }
        }

        return tmp;
    }
}