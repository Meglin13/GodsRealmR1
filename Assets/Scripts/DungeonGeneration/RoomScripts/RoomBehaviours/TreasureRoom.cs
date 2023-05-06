using UnityEngine;

[CreateAssetMenu(fileName = "Treasure Room", menuName = "Objects/Dungeon Generator/RoomBehaviour/Treasure Room")]
public class TreasureRoom : RoomBehaviour
{
    public TreasureRoom()
    {
        BehaviourType = RoomBehaviourType.FreeChest;
    }

    public override void Initialize(RoomScript room)
    {
        base.Initialize(room);
    }

    public override void OnRoomEnter()
    {
        
    }

    public override void OnRoomExit()
    {
        
    }
}