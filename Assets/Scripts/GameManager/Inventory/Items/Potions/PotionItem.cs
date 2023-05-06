public enum PotionType
{
    Health, Stamina, Mana,
    Modifier
}

public enum PotionStrength
{
    Weak, Mid, Strong
}


public class PotionItem : Item
{
    public PotionItem()
    {
        Type = ItemType.Potion;
        IsStackable = true;
    }

    public PotionType PotionType;
    public PotionStrength PotionStrength;

    public override void UseItem(CharacterScript character)
    {
        InventoryScript.Instance.DeleteItem(this.ID, true);
    }
}