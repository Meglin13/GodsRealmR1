using MyBox;
using System;
using System.Data.SqlTypes;

public enum ModType { Buff, Debuff }
public enum ModifierAmountType { Procent, Value }

[Serializable]
public class Modifier
{
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

    public Modifier(StatType StatType, float Amount, ModType modType) : this (StatType, Amount)
    {
        ModifierType = modType;
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

    public string Name;
    public string Description;
    public ModType ModifierType;
    public StatType StatType;
    [ConditionalField(nameof(StatType), false, new object[2] { StatType.ElementalDamageBonus, StatType.Resistance } )]
    public Element Element;
    public ModifierAmountType ModifierAmountType;
    public float Amount;
    public bool IsPermanent = false;
    public bool IsVisible = true;
    [ConditionalField(nameof(IsPermanent), inverse: true)]
    public float DurationInSecs;
}