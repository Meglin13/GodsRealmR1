using UnityEngine;

public enum RoomBehaviourType { Enemy, Shop, Buffer, FreeChest, Corridor }

public class RoomBehaviour : ScriptableObject
{
    public RoomBehaviourType BehaviourType;
}