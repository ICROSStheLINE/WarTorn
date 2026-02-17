using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField]
    private List<Item> items;

    public Inventory()
    {
        items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log($"Added {item.itemName} to inventory.");
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Removed {item.itemName} from inventory.");
        }
        else
        {
            Debug.Log($"Item {item.itemName} not found in inventory.");
        }
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public Item this[int i]
    {
        get { return items[i]; }
        set { items[i] = value; }
    }
}