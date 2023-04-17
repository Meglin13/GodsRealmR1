using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    private int EnemiesAmount;
    private int EnemiesLeft;

    private List<EnemyScript> Enemies;

    public void Init(int enemiesAmount)
    {
        this.EnemiesAmount = enemiesAmount;
    }

    public event Action OnEnemiesClear = delegate { };

    void Update()
    {
        if (EnemiesLeft < EnemiesAmount)
        {

        }
        else
        {
            OnEnemiesClear();
            Destroy(gameObject);
        }
    }

    private void SpawnEnemy()
    {

    }
}