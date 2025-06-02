using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayIngredientDetector : MonoBehaviour
{
    [SerializeField] private GameObject[] trayIngredientPrefabs;
    
    private bool isPositionedOnTable = false;
    private bool[] ingredientAdded = new bool[5];

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            isPositionedOnTable = true;
            Debug.Log("Tray positioned on table");
            return;
        }

        if (!isPositionedOnTable) return;

        if (collision.gameObject.CompareTag("Lettuce") && !ingredientAdded[0] && trayIngredientPrefabs[0] != null)
        {
            AddIngredientToTray(0, collision.gameObject, "Lettuce");
        }
        else if (collision.gameObject.CompareTag("Burger") && !ingredientAdded[1] && trayIngredientPrefabs[1] != null)
        {
            AddIngredientToTray(1, collision.gameObject, "Burger");
        }
        else if (collision.gameObject.CompareTag("Bread") && !ingredientAdded[2] && trayIngredientPrefabs[2] != null)
        {
            AddIngredientToTray(2, collision.gameObject, "Bread");
        }
        else if (collision.gameObject.CompareTag("Onion") && !ingredientAdded[3] && trayIngredientPrefabs[3] != null)
        {
            AddIngredientToTray(3, collision.gameObject, "Onion");
        }
        else if (collision.gameObject.CompareTag("Tomato") && !ingredientAdded[4] && trayIngredientPrefabs[4] != null)
        {
            AddIngredientToTray(4, collision.gameObject, "Tomato");
        }
    }

    private void AddIngredientToTray(int index, GameObject ingredient, string ingredientName)
    {
        ingredientAdded[index] = true;
        trayIngredientPrefabs[index].SetActive(true);
        Destroy(ingredient);
        Debug.Log($"{ingredientName} added to tray and destroyed");
    }

}
