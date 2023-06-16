using Akaal.PvCustomizer.Scripts;
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
public class Item : ScriptableObject, ILocalizable
{
#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        id = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));

        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        this._Name = Path.GetFileNameWithoutExtension(assetPath);
        this._Description = Path.GetFileNameWithoutExtension(assetPath) + "_Desc";
    }
#endif

    [ReadOnly]
    [SerializeField]
    private string id;
    public string ID { get { return id; } }

    [ReadOnly]
    public string InstanceID;
    public static int lastID = 0;

    public string Name => _Name;
    public string Description => _Description;

    [ReadOnly]
    [SerializeField]
    private string _Name;
    [ReadOnly]
    [SerializeField]
    private string _Description;

    [PvIcon]
    public Sprite Icon;
    public Rarity _Rarity;
    [ReadOnly]
    public int Amount = 1;
    [ReadOnly]
    public bool IsStackable;
    [ReadOnly]
    public ItemType Type;

    public Item GetCopy()
    {
        var item = Instantiate(this);

        item.InstanceID = item.GetType().ToString()[0] + "_" + lastID;
        lastID++;

        return item;
    }

    public virtual void UseItem(CharacterScript character) { }
}