using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public int quantity;
    public const int maxQuantity = 32; // Maximum quantity for items.
    public GameObject gameObjectWithSeedBag; // Reference to the seed bag game object.
    public Item seedItem; // Reference to the seed item associated with this crop item.

    public Item(string name, Sprite itemIcon)
    {
        itemName = name;
        icon = itemIcon;
        quantity = 1; // Initialize quantity to 1 when an item is added.
    }

    // Function to increment the item quantity.
    public void AddItem()
    {
        if (quantity < maxQuantity) // Max quantity is 32.
        {
            quantity++;
        }
    }

    public void RemoveItem()
    {
        if (quantity < maxQuantity) // Max quantity is 32.
        {
            quantity--;
        }
    }
}
