using MyBox;
using System;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Potion
}

public enum Rarity
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

[Serializable]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID { get { return id; } }

    public string Name;
    public string Description;
    public Sprite Icon;
    public Rarity _Rarity;
    [HideInInspector]
    public int Amount = 1;
    public bool IsStackable;
    [ReadOnly]
    public ItemType Type;

    #if UNITY_EDITOR

    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

    #endif
}