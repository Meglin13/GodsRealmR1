using UnityEngine;

namespace DungeonGeneration
{
    [CreateAssetMenu(fileName = "Battle Room", menuName = "Objects/Dungeon Generator/RoomBehaviour/Battle Room")]
    public class BattleRoom : RoomBehaviour
    {
        public EnemyScript[] enemies;

        public override void Initialize(RoomScript room)
        {
            base.Initialize(room);

            SetProp(false);
        }

        public override void OnRoomEnter()
        {
            if (!Room.IsRoomCleared)
            {
                Room.SetDoorsState(false);
                
                if (InstantiatingProp.TryGetComponent(out EnemySpawnerScript enemySpawner))
                {
                    enemySpawner.Init(this);
                }
            }
        }

        public override void OnRoomExit()
        {
            
        }
    } 
}