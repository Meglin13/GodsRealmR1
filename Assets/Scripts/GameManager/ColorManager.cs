using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour, IManager
{
    public static ColorManager Instance;

    [ColorUsage(false, false)]
    public Color EnemyMarkerColor, CharMarkerColor;

    [ColorUsage(false, false)]
    public Color EnemyHealthColor;

    [ColorUsage(false, false)]
    public Color PlayerHealthColor, PlayerManaColor, PlayerStaminaColor;

    public Dictionary<Rarity, Color> RarityColor = new Dictionary<Rarity, Color>()
    {
        { Rarity.Common, Color.grey },
        { Rarity.Uncommon, Color.green },
        { Rarity.Rare, Color.blue },
        { Rarity.Epic, Color.magenta },
        { Rarity.Legendary, Color.yellow },
    };

    //TODO: Сделать цвета
    public Dictionary<Element, string> ElementColor = new Dictionary<Element, string>()
    {
        { Element.Fire, "E95400" },
        { Element.Water, "1A5ABA" },
        { Element.Earth, "652A00" },
        { Element.Air, "4BDDFD" },
        { Element.Dark, "23233B" },
        { Element.Light, "FFC700" },
    };

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Instance = this;
    }
}
