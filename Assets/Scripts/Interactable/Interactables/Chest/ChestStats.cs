using UnityEngine;

[CreateAssetMenu(fileName = "ChestStats", menuName = "Objects/Chest Stats")]
public class ChestStats : ScriptableObject
{
    [Header("Info")]
    public Rarity ChestType;

    //Диапазон выпадаемых предметов
    [Header("Items")]
    public int MaxItems;
    public int MinItems;

    [Header("Gold")]
    public int MaxGold;
    public int MinGold;
}