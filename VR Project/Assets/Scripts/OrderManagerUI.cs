using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderManagerUI : MonoBehaviour
{
    [SerializeField] private Transform burgerContainer;
    [SerializeField] private Transform burgerIconTemplate;
    [SerializeField] private Transform drinkContainer;
    [SerializeField] private Transform drinkIconTemplate;
    [SerializeField] private Transform friesContainer;
    [SerializeField] private Transform friesIconTemplate;

    private void Awake()
    {
        burgerIconTemplate.gameObject.SetActive(false);
        drinkIconTemplate.gameObject.SetActive(false);
        friesIconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(OrderRecipeSO recipeSO)
    {
        foreach (Transform child in burgerContainer)
        {
            if (child == burgerIconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Transform child in drinkContainer)
        {
            if (child == drinkIconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Transform child in friesContainer)
        {
            if (child == friesIconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (OrderItemSO itemSO in recipeSO.itemSOList)
        {
            Transform iconTransform = null;

            switch (itemSO.itemType)
            {
                case OrderItemType.Drink:
                    iconTransform = Instantiate(drinkIconTemplate, drinkContainer);
                    break;
                case OrderItemType.Fries:
                    iconTransform = Instantiate(friesIconTemplate, friesContainer);
                    break;
            }

            if (iconTransform != null)
            {
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<Image>().sprite = itemSO.itemImage;
            }
        }

        OrderItemSO[] burgersByOrder = new OrderItemSO[6];
        if (recipeSO.burgerRecipeSO != null)
        {
            foreach (OrderItemSO itemSO in recipeSO.burgerRecipeSO.itemSOList)
            {
                if (itemSO.itemOrder >= 0 && itemSO.itemOrder < 6)
                {
                    burgersByOrder[itemSO.itemOrder] = itemSO;
                }
            }
        }

        for (int i = 0; i < 6; i++)
        {
            Transform iconTransform = Instantiate(burgerIconTemplate, burgerContainer);
            iconTransform.gameObject.SetActive(true);

            Image img = iconTransform.GetComponent<Image>();
            if (burgersByOrder[i] != null)
            {
                img.sprite = burgersByOrder[i].itemImage;
                var color = img.color;
                color.a = 1f;
                img.color = color;
            }
            else
            {
                var color = img.color;
                color.a = 0f;
                img.color = color;
            }
        }
    }
}
