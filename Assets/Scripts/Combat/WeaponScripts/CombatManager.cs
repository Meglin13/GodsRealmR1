using System;
using UnityEngine;

public enum Element { Fire, Water, Earth, Air, Dark, Light }

public class CombatManager : MonoBehaviour, IManager
{
    public static IManager Instance { get; private set; }

    public void Initialize()
    {
        Instance = this;
    }

    private void Awake()
    {
        Initialize();
    }

    //TODO: Бонусы урона по элементам
    private static float[,] ElementsDamageRatios = new float[6, 6];

    public static float DamageCalc(EntityStats Target, EntityStats Dealer, float Multiplier)
    {
        Element TargetElement = Target.Element;
        float TargetDefense = Target.ModifiableStats[StatType.Defence].GetFinalValue();
        float TargetResistance = Target.ElementsResBonus[TargetElement].Resistance.GetFinalValue() / 100;

        float DealerDamageBonus = Dealer.ElementsResBonus[Dealer.Element].DamageBonus.GetFinalValue() / 100;
        float DealerAttack = Dealer.ModifiableStats[StatType.Attack].GetFinalValue();

        float ElementRatioDamage = ElemDamageCalc(Dealer.Element, TargetElement);

        float result = (1 - TargetResistance + DealerDamageBonus + ElementRatioDamage) * (DealerAttack * (100 / (100 + TargetDefense))) * Multiplier / 100;
        
        result = (float)Math.Round(result);
        return result;
    }

    public static float ElemDamageCalc(Element Dealer, Element Target)
    {
        return (ElementsDamageRatios[(int)Dealer, (int)Target]) / 100;
    }


}