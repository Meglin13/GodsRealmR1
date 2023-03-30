using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum PotionType
{ 
	Health, Stamina, Mana, Modifier
}

//TODO: Зелья, протестировать
[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Objects/Potion")]
public class PotionItem : Item
{
	public PotionItem()
	{
		Type = ItemType.Potion;
		IsStackable = true;
	}

    public PotionType PotionType;
    [SerializeField]
    [ConditionalField(nameof(PotionType), false, new object[3] { PotionType.Health, PotionType.Mana, PotionType.Stamina })]
	private float AmountOfPotion;

    [SerializeField]
	[ConditionalField(nameof(PotionType), false, PotionType.Modifier)]
    private Modifier Modifier;

	public override void UseItem(CharacterScript character)
	{
		if (PotionType == PotionType.Modifier)
		{
            character.StartCoroutine(character.AddModifier(Modifier));
        }
		else
		{
			character.GiveSupport(AmountOfPotion, (StatType)(int)PotionType);
		}
    }
}