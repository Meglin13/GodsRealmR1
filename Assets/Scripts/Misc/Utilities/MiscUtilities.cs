using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Класс для дополнительных разномастных функций
/// </summary>
public class MiscUtilities : MonoBehaviour
{
    public static MiscUtilities Instance;
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
        var DamagePopUp = Instantiate(GameManager.Instance.DamagePopUp, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)), Quaternion.Euler(0, 0, 0));
        DamagePopUp.transform.SetParent(MiscUtilities.Instance.Dump.transform);

        DamagePopUp.transform.localScale *= Scale;
        DamagePopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"<color={ColorString}>{Text}</color>";
        Destroy(DamagePopUp, 1f);
    }

    public void ThrowThrowable(GameObject throwable, IDamageable dude, Skill skill)
    {
        var gm = Instantiate(throwable, dude.gameObject.transform.position + dude.gameObject.transform.forward + dude.gameObject.transform.up, dude.gameObject.transform.rotation);
        gm.GetComponent<ThrowableScript>().Init(dude, skill);
        gm.transform.parent = MiscUtilities.Instance.Dump.transform;
    }

    /// <summary>
    /// Спавнит игровой объект перед другим игровым объектом на заданном расстоянии от него
    /// </summary>
    /// <param name="spawner">Тот, кто спавнит</param>
    /// <param name="spawningObject">То, что спавнят</param>
    /// <param name="distance">Расстояние между тем, кто спавнит, и тем, что спавнят</param>
    public static GameObject SpawnObjectInFrontOfObject(GameObject spawner, GameObject spawningObject, float spawnDistance)
    {
        Vector3 playerPos = spawner.transform.position;
        Vector3 playerDirection = spawner.transform.forward;
        Quaternion playerRotation = spawner.transform.rotation;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        return Instantiate(spawningObject, spawnPos, playerRotation);
    }

    /// <summary>
    /// Отложенное выполнение функции
    /// </summary>
    /// <param name="delaySeconds">Время, через которое будет вызван метод</param>
    /// <param name="action">Вызывемый метод</param>
    /// <returns></returns>
    public IEnumerator ActionWithDelay(float delaySeconds, Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    public string GetCurrentClipName(Animator animator)
    {
        var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        return clipInfo[0].clip.name;
    }

    /// <summary>
    /// Метод для получения следующего индекса списка. 
    /// Если следующий индекс выходит за список, то он будет равен 0.
    /// </summary>
    /// <typeparam name="T">Универсальный тип</typeparam>
    /// <param name="list">Список</param>
    /// <param name="curIndex">Текущий индекс</param>
    /// <returns></returns>
    public void NextIndex<T>(List<T> list, ref int curIndex)
    {
        curIndex++;

        if (curIndex > list.Count - 1)
            curIndex = 0;
    }

    public void PreviousIndex<T>(List<T> list, ref int curIndex)
    {
        curIndex--;

        if (curIndex < 0)
            curIndex = list.Count - 1;
    }
}