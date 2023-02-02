using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCellScript : MonoBehaviour
{
    private Item Item;
    public Image ItemImage;
    public TextMeshProUGUI Amount;

    public void Initialize(Item item)
    {
        if (item)
        {
            Item = item;
            ItemImage.color = Color.white;
            ItemImage.sprite = Item.Icon;
            Amount.text = Item.Amount.ToString();
        }
        else
        {
            Amount.text = "";
        }
    }
}