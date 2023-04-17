using MyBox;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum WorldSide { North = 1, East, South, West}

/// <summary>
/// ����� �������� ���������� � �������� � ������������ �� ������
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
    /// ��������� ����� ��� �������
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
    public Vector2Int Size;

    public RoomBehaviour Behaviour;
    public bool IsRoomCleared = false;

    public event Action OnRoomClear = delegate { };
    public event Action OnRoomEnter = delegate { };
    public event Action OnRoomExit = delegate { };

    public Vector2Int coordinates;

    [HideInInspector]
    public int OpenedDoorsCount;
    public List<DoorwayObject> DoorwaysList = new List<DoorwayObject>(4)
    {
        new DoorwayObject() { Side = WorldSide.North},
        new DoorwayObject() { Side = WorldSide.East},
        new DoorwayObject() { Side = WorldSide.South},
        new DoorwayObject() { Side = WorldSide.West}
    };

    public void OnRoomEnterTrigger()
    {
        //Debug.Log("Enter the " + gameObject.name);
        OnRoomEnter();
    }

    public void OnRoomExitTrigger()
    {
        //Debug.Log("Exit the " + gameObject.name);
        OnRoomEnter();
    }

    private void Awake()
    {
        SetDoorways();

        OnRoomClear += () => SetDoorsState(true);
    }

    public void RoomClear()
    {
        IsRoomCleared = true;
        OnRoomClear();
    }

    public void SetBehaviour(RoomBehaviour behaviour)
    {
        this.Behaviour = Instantiate(behaviour);
        Behaviour.Initialize(this);
    }

    #region [Doors]
    /// <summary>
    /// ��������� ��������� ������
    /// </summary>
    /// <param name="sides">������ ���������� � ��������� ��������. 
    /// ����� ����������� �� ������� �������, ������� � ������: �����, ������, ��, �����</param>
    public void SetDoorways(bool[] sides)
    {
        OpenedDoorsCount = 0;

        for (int i = 0; i < sides.Length; i++)
        {
            if (sides[i])
            {
                DoorwaysList[i].IsDoorActive = true;
                DoorwaysList[i].SetDoorway();
                OpenedDoorsCount++;
            }
        }
    }

    /// <summary>
    /// ��������� ��������
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
    /// ����� ������������ ��� ����, ����� ��������� ������ �����, � �� �������
    /// </summary>
    /// <param name="IsOpened">������� ����� ��� ���</param>
    public void SetDoorsState(bool IsOpened)
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
        SetDoorsState(false);
    }

    [ButtonMethod]
    public void OpenDoors()
    {
        SetDoorsState(true);
    }

#endif
    #endregion
}