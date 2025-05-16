using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Order System/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<OrderItemSO> itemSOList;
}
