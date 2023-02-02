using UnityEngine;

public enum ChestType
{
    Common, Rare, Epic, Legendary
}

public class ChestStats : ScriptableObject
{
    public ChestType ChestType;

    //Диапазон выпадаемых предметов
    public int MaxItems;
    public int MinItems;

    //Диапазон золота
    public int MaxGold;
    public int MinGold;
}