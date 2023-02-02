using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class CaptionManager : MonoBehaviour
{
    public static CaptionManager Instance;

    private float CaptionLifeTime;

    public ScrollView CaptionListSV;

    private void Awake()
    {
        Instance = this;
        CaptionLifeTime = GameManager.Instance.CaptionLifeTime;

        var root = GetComponent<UIDocument>().rootVisualElement;

        //CaptionListSV = root.Q<ScrollView>();

    }

    public void ShowCaption(string CaptionText)
    {

    }
}