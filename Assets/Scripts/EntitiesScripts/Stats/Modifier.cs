using MyBox;
using System;
using System.Data.SqlTypes;
using System.Diagnostics;

public enum ModType { Buff, Debuff }
public enum ModifierAmountType { Procent, Value }

[Serializable]
public class Modifier
{
    public Modifier()
    {

    }

    //Бафф на 1 секунду
    public Modifier(StatType StatType, float Amount)
    {
        IsVisible = false;
        DurationInSecs = 1;
        this.StatType = StatType;
        this.Amount = Amount;
        ModifierAmountType = ModifierAmountType.Value;
        ModifierType = ModType.Buff;
    }

    //Продолжительный модификатор
    public Modifier(StatType StatType, float Amount, ModifierAmountType amountType, float Duration, ModType modType)
    {
        IsVisible = true;
        DurationInSecs = Duration;
        this.StatType = StatType;
        this.Amount = Amount;
        ModifierAmountType = amountType;
        ModifierType = modType;
    }

    public Modifier(StatType StatType, float Amount, bool IsPermanent)
    {
        IsVisible = false;
        this.IsPermanent = IsPermanent;
        DurationInSecs = 0;
        this.StatType = StatType;
        this.Amount = Amount;
        ModifierAmountType = ModifierAmountType.Value;
        ModifierType = ModType.Buff;
    }

    public bool IsVisible = false;
    [ConditionalField(nameof(IsVisible))]
    public string Name;
    [ConditionalField(nameof(IsVisible))]
    public string Description;
    public ModType ModifierType;
    public StatType StatType;
    [ConditionalField(nameof(StatType), false, new object[2] { StatType.ElementalDamageBonus, StatType.Resistance } )]
    public Element Element;
    public ModifierAmountType ModifierAmountType;
    public float Amount;
    public bool IsPermanent = true;
    
    [ConditionalField(nameof(IsPermanent), inverse: true)]
    public float DurationInSecs = 0;
}