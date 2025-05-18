using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Order Recipe", menuName = "Order System/Order Recipe")]
public class OrderRecipeSO : ScriptableObject
{
    public List<OrderItemSO> itemSOList;
    public BurgerRecipeSO burgerRecipeSO;

    private void OnValidate()
    {
        if (itemSOList.Count > 2)
            itemSOList = itemSOList.GetRange(0, 2);
    }
}
