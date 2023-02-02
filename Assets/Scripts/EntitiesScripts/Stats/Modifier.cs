using MyBox;
using System;

public enum ModType
{
    Buff, Debuff
}

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
        ModifierType = ModType.Buff;
    }

    //Продолжительный модификатор
    public Modifier(StatType StatType, float Amount, float Duration, ModType modType)
    {
        IsVisible = true;
        DurationInSecs = Duration;
        this.StatType = StatType;
        this.Amount = Amount;
        ModifierType = modType;
    }

    public string Name;
    public string Description;
    public ModType ModifierType;
    public StatType StatType;
    public int AmountInProcent;
    public float Amount;
    public bool IsPermanent = false;
    public bool IsVisible = true;
    [ConditionalField(nameof(IsPermanent), inverse: true)]
    public float DurationInSecs;
}