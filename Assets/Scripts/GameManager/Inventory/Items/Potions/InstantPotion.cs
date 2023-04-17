using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantPotion", menuName = "Objects/Items/Potions/Instant Potion")]
public class InstantPotion : PotionItem
{
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

    public float AmountOfPotion;

    public override void UseItem(CharacterScript character)
    {
        base.UseItem(character);
        character.GiveSupport(AmountOfPotion, (StatType)(int)PotionType);
    }
}