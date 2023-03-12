using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO: Зелья
[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Objects/Potion")]
public class PotionItem : Item
{
	public PotionItem()
	{
		Type = ItemType.Potion;
		IsStackable = true;
	}

    public Modifier Modifier;
	public void Use(CharacterScript character)
	{

	}
}