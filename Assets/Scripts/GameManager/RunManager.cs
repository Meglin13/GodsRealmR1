using DungeonGeneration;
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
    public static int Difficulty = 1;
    public static RunParameters Params { get; set; }
    private static float dropChanceAffix = 1;

    public static int CurrentFloor = 1;

    private static Dictionary<Rarity, float> BaseDropChances = new Dictionary<Rarity, float>(5)
    {
        { Rarity.Common, 59 },
        { Rarity.Uncommon, 25 },
        { Rarity.Rare, 10 },
        { Rarity.Epic, 5 },
        { Rarity.Legendary, 1 },
    };

    public void NewFloor() => CurrentFloor++;
    public void EndRun() => CurrentFloor = 1;

    public static void SetDifficulty(int Difficulty)
    {
        RunManager.Difficulty = Difficulty;

        Params ??= new RunParameters()
        {
            EnemiesLevel = Difficulty * 7,
            EnemiesInOneRoom = Difficulty + 3,
            FloorsAmount = Mathf.FloorToInt(Difficulty * 0.5f)
        };

        if (Params.GenerationParameters == null)
        {
            Debug.Log("Setting gen params");
            SetGenerationParams();
        }

        dropChanceAffix = RunManager.Difficulty != 1 ? 1 + Difficulty / 40 : 1;
    }

    public static void SetDifficulty(int Difficulty, RunParameters param)
    {
        Params = param;
        SetDifficulty(Difficulty);
    }

    private static void SetGenerationParams()
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
            numberOfRooms = 9 * Difficulty / roomAff
        };

        p.numberOfEvents = Mathf.FloorToInt(p.numberOfRooms / 1.5f);

        Params.GenerationParameters = p;
    }

    public static void ResetSettings()
    {
        if (Params.GenerationParameters != null)
            Params.GenerationParameters = null; 
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