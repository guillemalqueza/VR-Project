using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayIngredientDetector : MonoBehaviour
{
    [SerializeField] private GameObject[] trayIngredientPrefabs;
    
    private bool isPositionedOnTable = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Table"))
        {
            isPositionedOnTable = true;
            Debug.Log("Tray positioned on table");
        }

        if (isPositionedOnTable)
        {
            if (collision.gameObject.CompareTag("Lettuce"))
            {
                trayIngredientPrefabs[0].SetActive(true);
                //Destroy(collision.gameObject);
                Debug.Log("Lettuce added to tray and destroyed");
            }
            else if (collision.gameObject.CompareTag("Burger"))
            {
                trayIngredientPrefabs[1].SetActive(true);
                //Destroy(collision.gameObject);
                Debug.Log("Burger added to tray and destroyed");
            }
            else if (collision.gameObject.CompareTag("Bread"))
            {
                trayIngredientPrefabs[2].SetActive(true);
                //Destroy(collision.gameObject);
                Debug.Log("Bread added to tray and destroyed");
            }
            else if (collision.gameObject.CompareTag("Onion"))
            {
                trayIngredientPrefabs[3].SetActive(true);
                //Destroy(collision.gameObject);
                Debug.Log("Onion added to tray and destroyed");
            }
            else if (collision.gameObject.CompareTag("Tomato"))
            {
                trayIngredientPrefabs[4].SetActive(true);
                //Destroy(collision.gameObject);
                Debug.Log("Tomato added to tray and destroyed");
            }
        }
    }
}
