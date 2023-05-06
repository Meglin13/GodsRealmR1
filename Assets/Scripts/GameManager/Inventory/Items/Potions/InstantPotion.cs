using UnityEngine;

[CreateAssetMenu(fileName = "InstantPotion", menuName = "Objects/Items/Potions/Instant Potion")]
public class InstantPotion : PotionItem
{
#if UNITY_EDITOR

    public override void OnValidate()
    {
        base.OnValidate();
        switch (PotionStrength)
        {
            case PotionStrength.Weak:
                AmountOfPotion = 10;
                this._Rarity = Rarity.Common;
                break;

            case PotionStrength.Mid:
                AmountOfPotion = 30;
                this._Rarity = Rarity.Uncommon;
                break;

            case PotionStrength.Strong:
                AmountOfPotion = 100;
                this._Rarity = Rarity.Rare;
                break;
        }
    }

#endif

    public float AmountOfPotion;

    public override void UseItem(CharacterScript character)
    {
        base.UseItem(character);
        character.GiveSupport(AmountOfPotion, (StatType)(int)PotionType);
    }
}