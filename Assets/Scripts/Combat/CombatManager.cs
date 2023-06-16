using System;
using UnityEngine;

public enum Element { Fire, Water, Earth, Air, Dark, Light }

public enum AreaType { Attack, Heal }

public class CombatManager
{
    private static float[,] ElementsDamageRatios = new float[6, 6]
    {
         //             Fire   Water  Earth  Air    Dark   Light
         /* Fire */   { 0,     0,     20,    0,     0,     0  },
         /* Water */  { 20,    0,     0,     0,     0,     0  },
         /* Earth */  { 0,     0,     0,     20,    0,     0  },
         /* Air */    { 20,    0,     20,    0,     0,     0  },
         /* Dark */   { 20,    20,    20,    20,    0,     30 },
         /* Light */  { 20,    20,    20,    20,    0,     30 }
    };


    public static float DamageCalc(EntityStats Target, EntityStats Dealer, float Multiplier)
    {
        Element TargetElement = Target.Element;
        float TargetDefense = Target.ModifiableStats[StatType.Defence].GetFinalValue();
        float TargetResistance = Mathf.Clamp01(Target.ElementsResBonus[TargetElement].Resistance.GetFinalValue() / 100);

        float DealerDamageBonus = Mathf.Clamp01(Dealer.ElementsResBonus[Dealer.Element].DamageBonus.GetFinalValue() / 100);
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