using UnityEngine;

public enum ChestType
{
    Common, Rare, Epic, Legendary
}

public class ChestStats : ScriptableObject
{
    public ChestType ChestType;

    //�������� ���������� ���������
    public int MaxItems;
    public int MinItems;

    //�������� ������
    public int MaxGold;
    public int MinGold;
}