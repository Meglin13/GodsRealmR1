using UnityEngine;

[CreateAssetMenu(fileName = "Modifier Potion", menuName = "Objects/Items/Potions/Modifier Potion")]
public class ModifierPotion : PotionItem
{
#if UNITY_EDITOR

    public override void OnValidate()
    {
        base.OnValidate();

        Modifier.Name = this.name + "_Buff";
        Modifier.Description = Modifier.Name + "_Desc";

        switch (PotionStrength)
        {
            case PotionStrength.Weak:
                Modifier.DurationInSecs = 60;
                Modifier.ModifierAmountType = ModifierAmountType.Procent;
                Modifier.Amount = 10;
                this._Rarity = Rarity.Common;
                break;

            case PotionStrength.Mid:
                Modifier.DurationInSecs = 180;
                Modifier.ModifierAmountType = ModifierAmountType.Procent;
                Modifier.Amount = 20;
                this._Rarity = Rarity.Uncommon;
                break;

            case PotionStrength.Strong:
                Modifier.DurationInSecs = 300;
                Modifier.ModifierAmountType = ModifierAmountType.Procent;
                Modifier.Amount = 30;
                this._Rarity = Rarity.Rare;
                break;
        }
    }

#endif

    [SerializeField]
    private Modifier Modifier;

    public ModifierPotion()
    {
        PotionType = PotionType.Modifier;
    }

    public override void UseItem(CharacterScript character)
    {
        base.UseItem(character);
        GameManager.Instance.StartCoroutine(character.AddModifier(Modifier));
    }
}