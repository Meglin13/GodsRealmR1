using MyBox;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum WorldSide { North = 1, East, South, West}

/// <summary>
/// Класс хранящий информацию о проходах и опредлеяющий их логику
/// </summary>
[Serializable]
public class DoorwayObject
{
    [ReadOnly]
    public WorldSide Side;

    public GameObject Wall;
    public GameObject Doorway;
    public GameObject Door;

    public bool IsDoorActive;

    /// <summary>
    /// Установка стены или прохода
    /// </summary>
    public void SetDoorway()
    {
        Wall.active = !IsDoorActive;
        Doorway.active = IsDoorActive;
    }

    public void SetDoor(bool IsOpened)
    {
        if (Doorway.active)
        {
            Door.active = !IsOpened;
        }
        else
        {
            Door.active = false;
        }
    }
}

[SelectionBase]
public class RoomScript : MonoBehaviour
{
    public bool IsRoomCleared = false;

    public event Action OnRoomClear = delegate { };
    public event Action OnRoomEnter = delegate { };
    public event Action OnRoomExit = delegate { };

    public List<DoorwayObject> DoorwaysList = new List<DoorwayObject>(4)
    {
        new DoorwayObject() { Side = WorldSide.North},
        new DoorwayObject() { Side = WorldSide.East},
        new DoorwayObject() { Side = WorldSide.South},
        new DoorwayObject() { Side = WorldSide.West}
    };

    private void Awake()
    {
        gameObject.isStatic = true;
        SetDoorways();

        OnRoomClear += () => SetDoors(true);
    }

    public void RoomClear()
    {
        IsRoomCleared = true;
        OnRoomClear();
    }

    public void SetBehaviour()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sides">Массив информации о ближайших комнатах. 
    /// Двери открываются по часовой стрелке, начиная с севера: СЕВЕР, ВОСТОК, ЮГ, ЗАПАД</param>
    public void SetDoorways(bool[] sides)
    {
        for (int i = 0; i < 4; i++)
        {
            if (sides[i])
            {
                DoorwaysList[i].IsDoorActive = true;
                DoorwaysList[i].SetDoorway();
            }
        }
    }

    /// <summary>
    /// Установка проходов
    /// </summary>
    [ButtonMethod]
    public void SetDoorways()
    {
        foreach (var door in DoorwaysList)
        {
            door.SetDoorway();
        }
    }

    /// <summary>
    /// Метод используется для того, чтобы закрывать именно двери, а не проходы
    /// </summary>
    /// <param name="IsOpened">Открыть двери или нет</param>
    public void SetDoors(bool IsOpened)
    {
        foreach (var door in DoorwaysList)
        {
            door.SetDoor(IsOpened);
        }
    }

#if UNITY_EDITOR

    [ButtonMethod]
    public void CloseDoors()
    {
        SetDoors(false);
    }

    [ButtonMethod]
    public void OpenDoors()
    {
        SetDoors(true);
    }

    [ButtonMethod]
    public void TestDoors()
    {
        SetDoorways(new bool[4] { true, false, false, false});
    }

#endif
}