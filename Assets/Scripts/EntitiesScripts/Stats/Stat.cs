using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ������ (���������) ���������
/// </summary>
[Serializable]
public class Stat
{
    /// <summary>
    /// ������� ��������
    /// </summary>
    [SerializeField]
    private float BaseValue;

    /// <summary>
    /// ������� �����
    /// </summary>
    private int Level { get; set; }

    /// <summary>
    /// ����������� ������
    /// </summary>
    [SerializeField]
    private float LevelModifier;

    /// <summary>
    /// ������ �������������
    /// </summary>
    [NonSerialized]
    private List<Modifier> Modifiers = new List<Modifier>();

    /// <summary>
    /// ����������� � ������� ���������, ������� � ������������� ������
    /// </summary>
    /// <param _name="baseValue"></param>
    /// <param _name="level"></param>
    /// <param _name="levelModifier"></param>
    public Stat(float baseValue, int level, float levelModifier)
    {
        BaseValue = baseValue;
        Level = level;
        LevelModifier = levelModifier;
    }

    /// <summary>
    /// ����������� ��� ����� ��� ����������� �� ������
    /// </summary>
    /// <param _name="baseValue">������� ��������</param>
    public Stat(float baseValue)
    {
        BaseValue = baseValue;
        Level = 0;
        LevelModifier = 0;
    }

    /// <summary>
    /// ����������� ��� �������� �������� � ������
    /// </summary>
    public Stat()
    {
        BaseValue = 0;
        Level = 0;
        LevelModifier = 0;
    }

    /// <summary>
    /// �������� ����� � ��������������
    /// </summary>
    /// <returns>��������� �������� ����� � �������� � ��������������</returns>
    public float GetFinalValue()
    {
        float ValueWithLevels = GetClearValue();
        float FinalValue = ValueWithLevels;

        foreach (var item in Modifiers)
        {
            float ModifierAmount;
            if (item.ModifierAmountType == ModifierAmountType.Procent)
                ModifierAmount = ValueWithLevels * (item.Amount / 100);
            else
                ModifierAmount = item.Amount;

            FinalValue += item.ModifierType == ModType.Buff ? +ModifierAmount : -ModifierAmount;
        }

        return FinalValue;
    }

    /// <summary>
    /// ���������� ������ ����� �� ������������ ���������� �������
    /// </summary>
    /// <param _name="LevelUp">���������� �������</param>
    public void NewLevel(int LevelUp)
    {
        if (this.Level == 0)
        {
            return;
        }
        Level += LevelUp;
    }

    /// <summary>
    /// ���������� ������ ����� �� ������
    /// </summary>
    /// <param _name="Level">�������</param>
    public void SetLevel(int Level)
    {
        if (this.Level == 0)
        {
            return;
        }

        this.Level = Level;
    }

    /// <summary>
    /// ���������� ������������
    /// </summary>
    /// <param _name="modifier">����������� �����������</param>
    public void AddModifier(Modifier modifier, bool ISStackable = false)
    {
        if (!Modifiers.Contains(modifier) | ISStackable)
            Modifiers.Add(modifier);
    }

    /// <summary>
    /// �������� ������������
    /// </summary>
    /// <param _name="modifier">��������� �����������</param>
    public void RemoveModifier(Modifier modifier)
    {
        if (Modifiers.Contains(modifier))
            Modifiers.Remove(modifier);
    }

    /// <summary>
    /// �������� �� ������� ������������
    /// </summary>
    /// <param name="modifier">�����������</param>
    /// <returns></returns>
    public bool ContainsModifier(Modifier modifier) => Modifiers.Contains(modifier);

    /// <summary>
    /// �������� ��������
    /// </summary>
    public void RemoveDebuffs() => Modifiers.RemoveAll(modifier => modifier.ModifierType == ModType.Debuff & !modifier.IsPermanent);

    public void ClearMods() => Modifiers.Clear();

    /// <summary>
    /// ��������� �������� �� ������� �������� ��� �������������
    /// </summary>
    /// <returns>1% �� ������� ��������</returns>
    public float GetProcent() => GetClearValue() / 100;

    /// <summary>
    /// ������ �������� ����� � ������ �������
    /// </summary>
    /// <returns>������ �������� �����</returns>
    public float GetClearValue() => BaseValue + (LevelModifier / 100 * BaseValue * Level);
}