using UnityEngine;

[CreateAssetMenu(fileName = "New Order Item", menuName = "Order System/Order Item")]
public class OrderItem : ScriptableObject
{
    [Tooltip("Item Image")]
    public Sprite itemImage;
}