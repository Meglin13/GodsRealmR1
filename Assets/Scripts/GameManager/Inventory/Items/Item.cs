using MyBox;
using System;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Potion,
    Artefact
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

[Serializable]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Rarity Rarity;
    [HideInInspector]
    public int Amount;
    public bool IsStackable;
    [ReadOnly]
    public ItemType Type;
    public Sprite Icon;
    public GameObject Prefab;
    public int Weight;
}