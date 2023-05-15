using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGeneration
{
    [CreateAssetMenu(fileName = "Boss Room", menuName = "Objects/Dungeon Generator/RoomBehaviour/Battle Room")]
    internal class BossRoom : RoomBehaviour
    {
        [SerializeField]
        private EnemyScript Boss;

        public override void OnRoomEnter()
        {
            Room.SetDoorsState(false);
        }

        public override void OnRoomExit()
        {
            
        }
    }
}
