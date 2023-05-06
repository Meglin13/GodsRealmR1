using MyBox;
using UnityEngine;

public enum RoomBehaviourType { Battle, Shop, Buffer, FreeChest, Corridor, Heal, BossPortal, Misc }

public abstract class RoomBehaviour : ScriptableObject
{
    [ReadOnly]
    public RoomBehaviourType BehaviourType;

    public GameObject InstantiatingProp;
    internal RoomScript Room;

    [SerializeField]

    public virtual void Initialize(RoomScript room)
    {
        this.Room = room;
    }

    public void SetProp(bool Active = true)
    {
        if (InstantiatingProp)
        {
            InstantiatingProp = Instantiate(InstantiatingProp);
            InstantiatingProp.transform.position = Room.transform.position;
            InstantiatingProp.SetActive(Active);

            InstantiatingProp.transform.SetParent(Room.transform);
        }
    }

    public abstract void OnRoomEnter();
    public abstract void OnRoomExit();
}