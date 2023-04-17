using MyBox;
using System;
using System.IO;
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
#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        id = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));

        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        this.Name = Path.GetFileNameWithoutExtension(assetPath);
        this.Description = Path.GetFileNameWithoutExtension(assetPath) + "_Desc";
    }
#endif

    [ReadOnly]
    [SerializeField]
    private string id;
    public string ID { get { return id; } }

    [ReadOnly]
    public string Name;
    [ReadOnly]
    public string Description;
    public Sprite Icon;
    public Rarity _Rarity;
    [HideInInspector]
    public int Amount = 1;
    public bool IsStackable;
    [ReadOnly]
    public ItemType Type;

    public virtual void UseItem(CharacterScript character) { }
}