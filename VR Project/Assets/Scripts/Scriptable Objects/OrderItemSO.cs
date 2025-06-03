using UnityEngine;

public enum OrderItemType
{
    Burger,
    Drink,
    Fries
}

[CreateAssetMenu(fileName = "New Order Item", menuName = "Order System/Order Item")]
public class OrderItemSO : ScriptableObject
{
    [Tooltip("Item Image")]
    public Sprite itemImage;

    [Tooltip("Item Prefab")]
    public GameObject itemPrefab;

    [Tooltip("Item Type")]
    public OrderItemType itemType;

    [Tooltip("Item Order")]
    public int itemOrder;

    [Tooltip("Item Index")]
    public int itemIndex;
}