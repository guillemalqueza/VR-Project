using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderManagerUI : MonoBehaviour
{
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private Transform burgerContainer;
    [SerializeField] private Transform burgerIconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
        burgerIconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(OrderRecipeSO recipeSO)
    {
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Transform child in burgerContainer)
        {
            if (child == burgerIconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (OrderItemSO itemSO in recipeSO.itemSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = itemSO.itemImage;
        }

        if (recipeSO.burgerRecipeSO != null)
        {
            foreach (OrderItemSO itemSO in recipeSO.burgerRecipeSO.itemSOList)
            {
                Transform iconTransform = Instantiate(burgerIconTemplate, burgerContainer);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<Image>().sprite = itemSO.itemImage;
            }
        }
    }
}
