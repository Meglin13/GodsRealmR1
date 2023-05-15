using MyBox;
using ObjectPooling;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonGeneration
{
    public class EnemySpawnerScript : MonoBehaviour
    {
        [SerializeField]
        private float TimeBetweenSpawn = 3f;

        [SerializeField]
        [ReadOnly]
        private int EnemiesAmount;
        private int EnemiesLevel;

        [SerializeField]
        [ReadOnly]
        private int spawnedEnemiesCount = 0;

        public int SpawnedEnemiesCount 
        { 
            get => spawnedEnemiesCount; 
            set
            {
                spawnedEnemiesCount = value;
                if (spawnedEnemiesCount <= 0 & IsInited & !behaviour.Room.IsRoomCleared)
                    behaviour.Room.RoomClear();
            }
        }

        [SerializeField]
        [ReadOnly]
        private bool IsInited = false;

        private BattleRoom behaviour;

        public void Init(BattleRoom behaviour, int EnemiesAmount = 0)
        {
            IsInited = false;
            spawnedEnemiesCount = 0;

            this.behaviour = behaviour;

            this.EnemiesAmount = EnemiesAmount > 0 ? EnemiesAmount : RunManager.Params.EnemiesInOneRoom;
            EnemiesLevel = RunManager.Params.EnemiesLevel;

            gameObject.SetActive(true);
            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            int i = 0;

            while (i < EnemiesAmount)
            {
                yield return new WaitForSeconds(TimeBetweenSpawn);

                Vector3 position = gameObject.transform.position;
                position.x += Random.Range(-5, 5f);
                position.z += Random.Range(-5, 5f);

                var enemy = PoolerScript<EnemyScript>.Instance.
                    CreateObject(behaviour.enemies[Random.Range(0, behaviour.enemies.Length - 1)], position);

                enemy.agent.Warp(position);

                enemy.EntityStats.Initialize(Random.Range(EnemiesLevel - 2, EnemiesLevel + 5));
                
                enemy.OnDie -= OnEnemyDie;
                enemy.OnDie += OnEnemyDie;

                i++;
                SpawnedEnemiesCount++;
            }

            IsInited = true;
        }

        private void OnEnemyDie()
        {
            SpawnedEnemiesCount--;
        }
    }
}