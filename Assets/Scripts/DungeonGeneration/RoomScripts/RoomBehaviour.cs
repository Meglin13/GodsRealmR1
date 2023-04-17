using UnityEngine;

public enum RoomBehaviourType { Battle, Shop, Buffer, FreeChest, Corridor, Heal, BossPortal }

public class RoomBehaviour : ScriptableObject
{
    public readonly RoomBehaviourType BehaviourType;

    public GameObject Prop;
    public ChestScript Chest;
    public RoomScript Room;


    public virtual void Initialize(RoomScript room)
    {
        this.Room = room;
        SetProp();
    }

    public void SetProp()
    {
        GameObject prop = Instantiate(Prop);
        prop.transform.position = Room.transform.position;
    }

    public virtual void OnRoomEnter() { }
    public virtual void OnRoomExit() { }
}