using DungeonGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RunParameters
{
    public int Seed { get; set; } = 0;
    public int FloorsAmount { get; set; }
    public int EnemiesLevel { get; set; }
    public int EnemiesInOneRoom { get; set; }
    public GenerationParameters GenerationParameters { get; set; }
}

public class RunManager : MonoBehaviour
{
    public static int difficulty = 1;
    public static int Difficulty
    {
        get => difficulty;
        private set => difficulty = value;
    }

    public static RunParameters Params { get; private set; }
    private static float dropChanceAffix = 1;

    private static int currentFloor = 1;
    public static int CurrentFloor
    {
        get => currentFloor;
        private set => currentFloor = value;
    }

    private static Dictionary<Rarity, float> BaseDropChances = new Dictionary<Rarity, float>(5)
    {
        { Rarity.Common, 59 },
        { Rarity.Uncommon, 25 },
        { Rarity.Rare, 10 },
        { Rarity.Epic, 5 },
        { Rarity.Legendary, 1 },
    };

    public static void NewFloor() => CurrentFloor++;
    public static void EndRun() => CurrentFloor = 1;

    public static void SetDifficulty(int Difficulty, RunParameters param = null)
    {
        CurrentFloor = 1;

        RunManager.Difficulty = Difficulty;

        Params = param;

        Params ??= new RunParameters()
        {
            EnemiesLevel = Difficulty * 7,
            EnemiesInOneRoom = Difficulty + 3,
            FloorsAmount = (int)Math.Ceiling(Difficulty * 0.5f)
        };

        Params.GenerationParameters ??= SetGenerationParams();

        dropChanceAffix = RunManager.Difficulty != 1 ? 1 + Difficulty / 40 : 1;
    }

    private static GenerationParameters SetGenerationParams()
    {
        int roomAff = 2;

        if (Difficulty > 2)
            roomAff = 3;

        if (Difficulty > 10)
            roomAff = 4;

        GenerationParameters p = new GenerationParameters()
        {
            GridSize = new Vector2Int(10, 30),
            Seed = Params.Seed,
            numberOfRooms = 10 * Difficulty / roomAff
        };

        p.numberOfEvents = Mathf.FloorToInt(p.numberOfRooms / 1.5f);

        return p;
    }

    public static void ResetSettings()
    {
        Difficulty = 1;

        if (Params?.GenerationParameters != null)
        {
            Params = null;
            Params.GenerationParameters = null;
            CurrentFloor = 1;
        }
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