using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiscUtilities : MonoBehaviour
{
    public static MiscUtilities Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int Clamp(int value, int min, int max)
    {
        return (value < min) ? min : (value > max) ? max : value;
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

    public float ZabebaRandom(float Min, float Max)
    {
        float result = 0;

        return result;
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
        GameObject DamagePopUp = Instantiate(GameManager.Instance.DamagePopUp, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.Euler(0, 0, 0));
        DamagePopUp.transform.localScale *= Scale;
        DamagePopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"<color={ColorString}>{Text}</color>";
        Destroy(DamagePopUp, 1f);
    }


    public static void ThrowBullet(GameObject Bullet, float Mult, Vector3 GunPoint, IDamageable dude)
    {
        if (Bullet != null)
        {
            GameObject bullet = Instantiate(Bullet, GunPoint, dude.gameObject.transform.rotation);
            BulletsScript bulletsScript = bullet.GetComponent<BulletsScript>();
            bulletsScript.DealerStats = dude.EntityStats;
            bulletsScript.Mult = Mult;
            bullet.GetComponent<Rigidbody>().AddForce(bulletsScript.transform.forward * bulletsScript.Speed, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("No bullet!");
        }
    }

    public IEnumerator Cooldown(float delaySeconds, Action action)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    public string GetCurrentClipName(Animator animator)
    {
        int layerIndex = 0;
        var clipInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);
        return clipInfo[0].clip.name;
    }
}