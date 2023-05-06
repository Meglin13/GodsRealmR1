using System.Collections.Generic;


/// <summary>
/// Класс статов (атрибутов) сущностей
/// </summary>
public class Stat
{
    /// <summary>
    /// Базовое значение
    /// </summary>
    private float BaseValue { get; set; }

    /// <summary>
    /// Уровень стата
    /// </summary>
    private int Level { get; set; }

    /// <summary>
    /// Модификатор уровня
    /// </summary>
    private float LevelModifier { get; set; }

    /// <summary>
    /// Список модификаторов
    /// </summary>
    private List<Modifier> Modifiers = new List<Modifier>();

    /// <summary>
    /// Конструктор с базовым значением, уровнем и модификатором уровня
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
    /// Конструктор для стата без зависимости от уровня
    /// </summary>
    /// <param _name="baseValue">Базовое значение</param>
    public Stat(float baseValue)
    {
        BaseValue = baseValue;
        Level = 0;
        LevelModifier = 0;
    }

    /// <summary>
    /// Конструктор без базового значения и уровня
    /// </summary>
    public Stat()
    {
        BaseValue = 0;
        Level = 0;
        LevelModifier = 0;
    }

    /// <summary>
    /// Значение стата с модификаторами
    /// </summary>
    /// <returns>Финальное значение стата с уровнями и модификаторами</returns>
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
    /// Увеличение уровня стата на определенное количество уровней
    /// </summary>
    /// <param _name="LevelUp">Количество уровней</param>
    public void NewLevel(int LevelUp)
    {
        if (this.Level == 0)
        {
            return;
        }
        Level += LevelUp;
    }

    /// <summary>
    /// Увеличение уровня стата до уровня
    /// </summary>
    /// <param _name="Level">Уровень</param>
    public void SetLevel(int Level)
    {
        if (this.Level == 0)
        {
            return;
        }

        this.Level = Level;
    }

    /// <summary>
    /// Добавления модификатора
    /// </summary>
    /// <param _name="modifier">Добавляемый модификатор</param>
    public void AddModifier(Modifier modifier)
    {
        if (!Modifiers.Contains(modifier))
            Modifiers.Add(modifier);
    }

    /// <summary>
    /// Удаление модификатора
    /// </summary>
    /// <param _name="modifier">Удаляемый модификатор</param>
    public void RemoveModifier(Modifier modifier)
    {
        if (Modifiers.Contains(modifier))
            Modifiers.Remove(modifier);
    }

    /// <summary>
    /// Проверка на наличие модификатора
    /// </summary>
    /// <param _name="modifier">Модификатор</param>
    /// <returns></returns>
    public bool ContainsModifier(Modifier modifier) => Modifiers.Contains(modifier);

    /// <summary>
    /// Удаление дебаффов
    /// </summary>
    public void RemoveDebuffs() => Modifiers.RemoveAll(modifier => modifier.ModifierType == ModType.Debuff & !modifier.IsPermanent);

    /// <summary>
    /// Получение процента от чистого значения без модификаторов
    /// </summary>
    /// <returns>1% от чистого значения</returns>
    public float GetProcent() => GetClearValue() / 100;

    /// <summary>
    /// Чистое значение стата с учетом уровней
    /// </summary>
    /// <returns>Чистое значение стата</returns>
    public float GetClearValue() => BaseValue + (LevelModifier / 100 * BaseValue * Level);

    public string PrintModifiers()
    {
        string result = "";

        foreach (var item in Modifiers)
        {
            result += $"{item.Amount} {item.StatType}";
        }

        return result;
    }
}