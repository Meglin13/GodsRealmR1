using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour, IManager
{
    public static RunManager Instance { get; private set; }

    public static List<CharacterScript> Team = new List<CharacterScript>(3);

    public static int Difficulty = 1;
    public static int MaxFloors;
    public static int CurrentFloor;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        MaxFloors = Difficulty;
        CurrentFloor = 1;
    }

    public void NewFloor() => CurrentFloor++;

    public void FailRun() => CurrentFloor = 1;
}