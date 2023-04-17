using MyBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

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
		InventoryScript.Instance.DeleteItem(this.ID);
    }
}