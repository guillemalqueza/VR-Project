using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Category", menuName = "Order System/Item Category")]
public class ItemCategory : ScriptableObject
{
    [Tooltip("Item List")]
    public List<OrderItem> availableItems = new List<OrderItem>();
}