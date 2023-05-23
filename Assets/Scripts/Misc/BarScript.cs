using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private Image Bar;
    [SerializeField]
    private TextMeshProUGUI numbers;
    [SerializeField]
    private TextMeshProUGUI level;

    private void Awake()
    {
        Bar = GetComponent<Image>();
    }

    public void ChangeBarValue(float CurrentValue, float MaxValue)
    {
        Bar.fillAmount = CurrentValue / MaxValue;

        if (numbers)
            numbers.text = $"{Mathf.FloorToInt(CurrentValue)}/{Mathf.FloorToInt(MaxValue)}";
    }

    public void SetLevel(int levelInt)
    {
        if (level)
            level.text = levelInt.ToString();
    }
}