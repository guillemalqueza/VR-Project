using UnityEngine;

[CreateAssetMenu(fileName = "New Order Item", menuName = "Order System/Order Item")]
public class OrderItemSO : ScriptableObject
{
    [Tooltip("Item Image")]
    public Sprite itemImage;

    [Tooltip("Item Prefab")]
    public GameObject itemPrefab;
}