using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


/// <summary>
/// Класс для дополнительных разномастных функций
/// </summary>
public class MiscUtilities : MonoBehaviour
{
    public static MiscUtilities Instance;
    [HideInInspector]
    public GameObject Dump;

    private void OnEnable()
    {
        Instance = this;

        Dump = new GameObject("Dump");
    }

    public static GameObject FindChildWithTag(Transform transform, string tag)
    {
        GameObject Child = null;

        foreach (Transform item in transform)
        {
            if (item.CompareTag(tag))
                Child = item.gameObject;
        }
        return Child;
    }

    //Thx Jamora
    public static void GetInterfaces<T>(out List<T> resultList, GameObject objectToSearch) where T : class
    {
        MonoBehaviour[] list = objectToSearch.GetComponents<MonoBehaviour>();
        resultList = new List<T>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is T)
                resultList.Add((T)(object)mb);
        }
    }

    public static void DamagePopUp(Transform transform, string Text, string ColorString, float Scale)
    {
        var DamagePopUp = VFXPool.Instance.CreateObject(GameManager.Instance.DamagePopUp, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)));
        DamagePopUp.transform.localScale *= Scale;
        DamagePopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"<color={ColorString}>{Text}</color>";
    }

    /// <summary>
    /// Спавнит игровой объект перед другим игровым объектом на заданном расстоянии от него
    /// </summary>
    /// <param _name="spawner">Тот, кто спавнит</param>
    /// <param _name="spawningObject">То, что спавнят</param>
    /// <param _name="distance">Расстояние между тем, кто спавнит, и тем, что спавнят</param>
    public static T SpawnObjectInFrontOfObject<T>(IDamageable spawner, T spawningObject, float spawnDistance) where T : SpawningObject
    {
        Transform transform = spawner.gameObject.transform;

        Vector3 playerPos = transform.position;
        Vector3 playerDirection = transform.forward;
        Quaternion playerRotation = transform.rotation;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        T spawnedObject = (T)SpawnablePool.Instance.CreateObject(spawningObject, spawnPos);

        if (spawnedObject.TryGetComponent(out NavMeshAgent agent))
        {
            agent.Warp(spawnPos);
        }

        return spawnedObject;
    }

    /// <summary>
    /// Отложенное выполнение функции
    /// </summary>
    /// <param _name="delaySeconds">Время, через которое будет вызван метод</param>
    /// <param _name="action">Вызывемый метод</param>
    /// <returns></returns>
    public IEnumerator ActionWithDelay(float delaySeconds, Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    /// <summary>
    /// Метод для получения следующего индекса списка. 
    /// Если следующий индекс выходит за список, то он будет равен 0.
    /// </summary>
    /// <typeparam _name="T">Универсальный тип</typeparam>
    /// <param _name="list">Список</param>
    /// <param _name="curIndex">Текущий индекс</param>
    /// <returns></returns>
    public static int NextIndex<T>(List<T> list, int curIndex, IndexType indexType)
    {
        int index = curIndex;

        index += indexType == IndexType.Next ? +1 : -1;

        if (index < 0)
            index = list.Count - 1;
        else if (index > list.Count - 1)
            index = 0;

        return index;
    }
}