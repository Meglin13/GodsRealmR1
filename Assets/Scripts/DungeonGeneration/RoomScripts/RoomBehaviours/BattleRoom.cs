using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BattleRoom : RoomBehaviour
{
    private int EnemiesNumber;
    private int EnemiesLeft;

    public override void Initialize(RoomScript room)
    {
        base.Initialize(room);
        EnemiesNumber = UnityEngine.Random.RandomRange(RunManager.Difficulty, RunManager.Difficulty + 5) + 2;
    }

}