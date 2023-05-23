using DungeonGeneration.DungeonProps;
using UnityEngine;

namespace DungeonGeneration
{
    [CreateAssetMenu(fileName = "Battle Room", menuName = "Objects/Dungeon Generator/RoomBehaviour/Battle Room")]
    public class BattleRoom : RoomBehaviour
    {
        [SerializeField]
        private int EnemiesAmount = 0;
        public EnemyScript[] enemies;
        private bool IsInited = false;
        [SerializeField]
        private GameObject Reward;

        public override void Initialize(RoomScript room)
        {
            base.Initialize(room);

            SetProp(InstantiatingProp, true);

            Room.OnRoomClear += OnRoomClear;
        }

        private void OnRoomClear() => SetProp(Reward);

        public override void OnRoomEnter()
        {
            if (!Room.IsRoomCleared)
            {
                Room.SetDoorsState(false);
                
                if (InstantiatingProp.TryGetComponent(out EnemySpawnerScript enemySpawner) 
                    & !IsInited)
                {
                    enemySpawner.Init(this, EnemiesAmount);
                    IsInited = true;
                }
            }
        }

        public override void OnRoomExit()
        {
            
        }
    } 
}