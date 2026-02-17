using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    private static int idCounter = 0;

    [Header("Basic Info")]
    public string itemID = GenerateID();
    public string itemName;
    [TextArea] public string itemDescription;
    [Header("Item Properties")]
    public ItemType itemType;
    public Rarity itemRarity;

    [Header("Stacking")]
    public bool stackable = true;
    public int maxStackSize = 99;

    [Header("Visuals")]
    public Sprite itemIcon;
    public GameObject worldPrefab;
    public GameObject heldPrefab;

    [Header("Stats")]
    public int damage;
    public float range;
    public float attackSpeed;
    public bool hasDurability;
    public float durability;
    public float price;

    private static string GenerateID()
    {
        idCounter++;
        return $"wartorn:{idCounter}";
    }
}