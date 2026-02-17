using UnityEngine;
using UnityEngine.UI;

public class CurrentItemController : MonoBehaviour
{
    public Image itemIcon;
    public void SetCurrentItem(Item item)
    {
        itemIcon.sprite = item.itemIcon;
    }

    void OnEnable()
    {
        LocalPlayerRegistry.OnLocalPlayerRegistered += OnLocalPlayerRegistered;
    }

    void OnDisable()
    {
        LocalPlayerRegistry.OnLocalPlayerRegistered -= OnLocalPlayerRegistered;
    }

    private void OnLocalPlayerRegistered(PlayerStateManager player)
    {
        player.InventoryController.currentItemSlot = gameObject;
    }
}
