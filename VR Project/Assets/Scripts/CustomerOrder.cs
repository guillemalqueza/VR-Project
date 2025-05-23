using System.Collections.Generic;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Canvas orderCanvas;
    [SerializeField] private OrderManagerUI orderManagerUI;
    
    [Header("Recipes")]
    [SerializeField] private List<OrderRecipeSO> availableRecipes;

    private OrderRecipeSO currentRecipe;

    private void Awake()
    {
        orderCanvas.enabled = false;
    }

    public void CompleteOrder()
    {
        orderCanvas.enabled = false;
    }

    public OrderRecipeSO GetCurrentRecipe()
    {
        return currentRecipe;
    }

    public void RequestOrder()
    {
        Debug.Log("RequestOrder");

        if (availableRecipes.Count > 0)
        {
            currentRecipe = availableRecipes[Random.Range(0, availableRecipes.Count)];

            orderCanvas.enabled = true;
            orderManagerUI.SetRecipeSO(currentRecipe);
        }
    }
}