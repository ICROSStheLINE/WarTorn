using Unity.Netcode;
using UnityEngine;

public class InventoryController : NetworkBehaviour
{
    public Inventory Inventory;
    public GameObject currentItemSlot { get; set; }
    public int selectedItemIndex = 0;

    public void AddItem(Item item)
    {
        Inventory.AddItem(item);
    }

    public void RemoveItem(Item item)
    {
        Inventory.RemoveItem(item);
    }

    public void SetCurrentItem(Item item)
    {
        if (currentItemSlot == null)
        {
            Debug.LogWarning("Current item slot is not set.");
            return;
        }

        CurrentItemController currentItemController = currentItemSlot.GetComponent<CurrentItemController>();
        if (currentItemController != null)
        {
            currentItemController.SetCurrentItem(item);
        }
    }

    [ContextMenu("Set Item")]
    void DoSomething()
    {
        if (Inventory.GetItems().Count > selectedItemIndex)
        {
            SetCurrentItem(Inventory[selectedItemIndex]);
        }
        else
        {
            Debug.LogWarning("Inventory does not have enough items to set the current item.");
        }
    }
}
