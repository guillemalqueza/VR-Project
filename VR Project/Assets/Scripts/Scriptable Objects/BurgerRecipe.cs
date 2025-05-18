using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Burger Recipe", menuName = "Order System/Burger Recipe")]
public class BurgerRecipeSO : ScriptableObject
{
    public List<OrderItemSO> itemSOList;

    private void OnValidate()
    {
        if (itemSOList.Count > 5)
            itemSOList = itemSOList.GetRange(0, 5);
    }
}
