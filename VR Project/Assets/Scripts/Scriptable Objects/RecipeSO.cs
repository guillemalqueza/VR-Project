using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Order System/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<OrderItemSO> itemSOList;

    private void OnValidate()
    {
        if (itemSOList.Count > 3)
            itemSOList = itemSOList.GetRange(0, 3);
    }
}
