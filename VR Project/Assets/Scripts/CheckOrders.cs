using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOrders : MonoBehaviour
{
    [SerializeField] private Table table;
    [SerializeField] private NPCSpawner npcSpawner;

    public void CheckOrder()
    {
        TrayIngredientDetector trayDetector = GetTrayOnTable();
        CustomerOrder currentCustomerOrder = GetCurrentCustomerOrder();

        List<int> trayIndexes = trayDetector.GetAddedIngredientIndexes();
        List<int> recipeIndexes = GetRecipeIndexes(currentCustomerOrder);

        bool match = CompareIndexes(trayIndexes, recipeIndexes);

        Animator npcAnimator = null;
        if (currentCustomerOrder != null)
            npcAnimator = currentCustomerOrder.GetComponentInChildren<Animator>();

        if (match)
        {
            Debug.Log("Correct order!");
            if (npcAnimator != null)
                npcAnimator.SetTrigger("Happy");
        }
        else
        {
            Debug.Log("Incorrect order.");
            if (npcAnimator != null)
                npcAnimator.SetTrigger("Angry");
        }
    }

    private TrayIngredientDetector GetTrayOnTable()
    {
        if (table == null) return null;
        GameObject trayObj = table.GetFirstTray();
        if (trayObj == null) return null;
        return trayObj.GetComponent<TrayIngredientDetector>();
    }

    private CustomerOrder GetCurrentCustomerOrder()
    {
        if (npcSpawner == null) return null;
        return npcSpawner.GetCurrentCustomerOrder();
    }

    private List<int> GetRecipeIndexes(CustomerOrder customerOrder)
    {
        List<int> indexes = new List<int>();
        if (customerOrder.currentRecipe != null)
        {
            foreach (var item in customerOrder.currentRecipe.itemSOList)
            {
                indexes.Add(item.itemIndex);
            }

            if (customerOrder.currentRecipe.burgerRecipeSO != null)
            {
                foreach (var item in customerOrder.currentRecipe.burgerRecipeSO.itemSOList)
                {
                    indexes.Add(item.itemIndex);
                }
            }
        }
        return indexes;
    }

    private bool CompareIndexes(List<int> tray, List<int> recipe)
    {
        var recipeSet = new HashSet<int>(recipe);
        var traySet = new HashSet<int>(tray);

        foreach (int recipeIndex in recipeSet)
        {
            if (!traySet.Contains(recipeIndex))
                return false;
        }

        foreach (int trayIndex in traySet)
        {
            if (!recipeSet.Contains(trayIndex))
                return false;
        }

        return true;
    }
}
