using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] private Transform liquidVisual;
    [SerializeField] private float minFillAmount = 0;
    [SerializeField] private float maxFillAmount = 0.065f;
    
    private float fillAmount = 0f;
    private bool isFilled = false;

    public void UpdateFill(float amount)
    {
        fillAmount = amount;

        Vector3 pos = liquidVisual.localPosition;
        pos.y = Mathf.Lerp(minFillAmount, maxFillAmount, fillAmount);
        liquidVisual.localPosition = pos;

        if (fillAmount >= 1f && !isFilled)
        {
            isFilled = true;
        }
    }
}