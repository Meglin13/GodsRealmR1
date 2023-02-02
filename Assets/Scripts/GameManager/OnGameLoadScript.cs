using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameLoadScript : MonoBehaviour
{
    //TODO: Скрипт загрузки игры
    void Awake()
    {
        SettingsScript.SetScreen(PlayerPrefs.GetString("Resolution"), PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false, PlayerPrefs.GetInt("RefreshRate"));
    }
}