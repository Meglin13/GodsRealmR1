using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс хранящий цвета объектов
/// </summary>
public class ColorManager : MonoBehaviour, ISingle
{
    public static ColorManager Instance;

    [ColorUsage(false, false)]
    public Color EnemyMarkerColor, CharMarkerColor;

    [ColorUsage(false, false)]
    public Color EnemyHealthColor;

    [ColorUsage(false, false)]
    public Color PlayerHealthColor, PlayerManaColor, PlayerStaminaColor;

    public Dictionary<Rarity, Color> RarityColor = new()
    {
        { Rarity.Common, Color.grey },
        { Rarity.Uncommon, Color.green },
        { Rarity.Rare, Color.blue },
        { Rarity.Epic, Color.magenta },
        { Rarity.Legendary, Color.yellow },
    };

    public Dictionary<Element, string> ElementColor = new()
    {
        { Element.Fire, "#E95400" },
        { Element.Water, "#1A5ABA" },
        { Element.Earth, "#652A00" },
        { Element.Air, "#4BDDFD" },
        { Element.Dark, "#23233B" },
        { Element.Light, "#FFC700" },
    };

    public Dictionary<StatType, string> StatsColor = new()
    {
        { StatType.Mana, "#001FFF" },
        { StatType.Health, "#45DA00" },
        { StatType.Stamina, "#FFF300" },
        { StatType.Speed, "#FFF300"},
        { StatType.Attack, "#F78502"},
        { StatType.Defence, "#0095FF"},
        { StatType.CritChance, "#ff9494"},
        { StatType.CritDamage, "#f00"},
        { StatType.ManaConsumption, "#001FFF"},
        { StatType.ManaRecoveryBonus, "#001FFF"},
        { StatType.InventorySlots, "#FFFFFF"},
    };

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Instance = this;
    }

    public static Color ChangeHue(ref float hue, float speed, float deltaTime)
    {
        hue += speed * deltaTime;

        if (hue > 1f | hue < 0f)
            hue = 0f;

        return Color.HSVToRGB(hue, 0.7f, 1);
    }

    //public static void GetColor()
    //{
    //    string statKey;
    //    Color col;

    //    if (modifier.StatType == StatType.Resistance | modifier.StatType == StatType.ElementalDamageBonus)
    //    {
    //        if (modifier.StatType == StatType.ElementalDamageBonus)
    //            statKey = modifier.Element + "DamageBonus";
    //        else
    //            statKey = modifier.Element + "Resistance";

    //        StatIcon.sprite = Resources.Load<Sprite>($"Icons/ModifierIcons/{modifier.Element}");

    //        if (ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.ElementColor[modifier.Element], out col))
    //            StatIcon.tintColor = col;
    //    }
    //    else
    //    {
    //        if (ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.StatsColor[modifier.StatType], out col))
    //            StatIcon.tintColor = col;

    //        StatIcon.sprite = Resources.Load<Sprite>($"Icons/ModifierIcons/{modifier.StatType}");

    //        statKey = modifier.StatType.ToString();
    //    }
    //}
}