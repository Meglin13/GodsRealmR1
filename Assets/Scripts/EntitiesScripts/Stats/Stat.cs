using System.Collections.Generic;

public class Stat
{
    //������� ��������
    private float BaseValue { get; set; }
    //�������
    private int Level { get; set; }
    //����������� � ��������� ��� ������������ ���������� �������� �����
    private float LevelModifier { get; set; }
    //������ �������������
    private List<Modifier> Modifiers = new List<Modifier>();

    public Stat(float baseValue, int level, float levelModifier)
    {
        BaseValue = baseValue;
        Level = level;
        LevelModifier = levelModifier;
    }

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
        Level = 0;
        LevelModifier = 0;
    }

    public Stat()
    {
        BaseValue = 0;
        Level = 0;
        LevelModifier = 0;
    }

    //�������� ����� � ��������������
    public float GetFinalValue()
    {
        float ValueWithLevels = BaseValue + (LevelModifier / 100 * BaseValue * Level);
        float FinalValue = ValueWithLevels;

        foreach (var item in Modifiers)
        {
            float ModifierAmount = ValueWithLevels * (float)item.AmountInProcent / 100 + (float)item.Amount;
            FinalValue += item.ModifierType == ModType.Buff ? +ModifierAmount : -ModifierAmount;
        }

        return FinalValue;
    }

    //���������� ������ �����
    public void NewLevel(int LevelUp) => Level += LevelUp;
    public void SetLevel(int Level) => this.Level = Level;

    //��������� �������������
    public void AddModifier(Modifier modifier)
    {
        if (!Modifiers.Contains(modifier))
            Modifiers.Add(modifier);
    }

    public void RemoveModifier(Modifier modifier) => Modifiers.Remove(modifier);
    public bool ContainsModifier(Modifier modifier) => Modifiers.Contains(modifier);

    //�������� ��������
    public void RemoveDebuffs() => Modifiers.RemoveAll(modifier => modifier.ModifierType == ModType.Debuff & !modifier.IsPermanent);

    //��������� �������� �� ������� �������� ��� �������������
    public float GetProcent() => GetClearValue() / 100;

    //������ �������� �����
    public float GetClearValue() => BaseValue + (LevelModifier / 100 * BaseValue * Level);
}