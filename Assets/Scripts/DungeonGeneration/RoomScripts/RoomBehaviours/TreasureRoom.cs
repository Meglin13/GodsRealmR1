using UnityEngine;

namespace DungeonGeneration
{
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
            SetProp(InstantiatingProp, true);
        }

        public override void OnRoomEnter()
        {

        }

        public override void OnRoomExit()
        {

        }
    } 
}