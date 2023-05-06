using DungeonGeneration;
using MyBox;
using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField]
    private float TimeBetweenSpawn = 5f;

    [SerializeField]
    [ReadOnly]
    private int EnemiesAmount;
    private int EnemiesLevel;

    private List<EnemyScript> SpawnedEnemies;

    private BattleRoom behaviour;

    public void Init(BattleRoom behaviour)
    {
        this.behaviour = behaviour;
        gameObject.SetActive(true);

        EnemiesAmount = RunManager.Params.EnemiesInOneRoom;
        EnemiesLevel = RunManager.Params.EnemiesLevel;

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        int i = 0;

        while (i < EnemiesAmount)
        {
            Vector3 position = gameObject.transform.position;
            //position.x += Random.Range(-1, 1);
            //position.z += Random.Range(-1, 1);

            var enemy = PoolerScript<EnemyScript>.Instance.
                CreateObject(behaviour.enemies[Random.Range(0, behaviour.enemies.Length - 1)], position);

            enemy.agent.Warp(position);

            enemy.EntityStats.Initialize(Random.Range(EnemiesLevel - 2, EnemiesLevel + 5));
            enemy.OnDie += () => OnEnemyDie(enemy);

            i++;

            yield return new WaitForSeconds(TimeBetweenSpawn);
        }
    }

    //EXP: Не работает
    private void OnEnemyDie(EnemyScript enemy)
    {
        SpawnedEnemies.Remove(enemy);

        if (SpawnedEnemies.Count == 0)
        {
            behaviour.Room.RoomClear();

            gameObject.SetActive(false);
        }
    }
}