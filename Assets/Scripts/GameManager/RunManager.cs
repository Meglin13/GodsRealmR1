using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour, IManager
{
    public static RunManager Instance { get; private set; }

    public static List<CharacterScript> Team = new List<CharacterScript>(3);

    public static int Difficulty { get; set; }
    private static float dropChanceAffix = 1;
    public static int MaxFloors;
    public static int CurrentFloor;

    private static Dictionary<Rarity, float> BaseDropChances = new Dictionary<Rarity, float>(5)
    {
        { Rarity.Common, 59 },
        { Rarity.Uncommon, 25 },
        { Rarity.Rare, 10 },
        { Rarity.Epic, 5 },
        { Rarity.Legendary, 1 },
    };

    private void Awake()
    {
        Initialize();
        SetDifficulty(1);
    }

    public void Initialize()
    {
        MaxFloors = Difficulty;
        CurrentFloor = 1;
    }

    public void NewFloor() => CurrentFloor++;
    public void FailRun() => CurrentFloor = 1;

    public static void SetDifficulty(int Difficulty)
    {
        RunManager.Difficulty = Difficulty;
        dropChanceAffix = RunManager.Difficulty != 1 ? 1 + Difficulty / 40 : 1;
    }

    public static float GetChance(Rarity rarity)
    {
        float dropChance = BaseDropChances[rarity];

        if (rarity != Rarity.Common)
            dropChance = dropChance * dropChanceAffix + (float)Difficulty / (float)rarity;
        else
        {
            float temp = 0;

            for (int i = 2; i < 6; i++)
            {
                temp += BaseDropChances[(Rarity)i] * dropChanceAffix + (float)Difficulty / (float)i;
            }

            dropChance = 100f - temp;
        }

        return dropChance;
    }
}