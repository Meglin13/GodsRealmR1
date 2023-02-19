using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private Image Bar;
    [SerializeField]
    private TextMeshProUGUI numbers;

    private void Awake()
    {
        Bar = GetComponent<Image>();
    }

    public void ChangeBarValue(float CurrentValue, float MaxValue)
    {
        Bar.fillAmount = CurrentValue / MaxValue;

        if (numbers)
        {
            numbers.text = $"{CurrentValue} / {MaxValue}";
        }
    }

    public void SetValue(float value)
    {
        Bar.fillAmount = value;
    }
}