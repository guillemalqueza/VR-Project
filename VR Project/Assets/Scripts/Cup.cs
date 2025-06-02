using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class Cup : MonoBehaviour
{
    [SerializeField] private Transform liquidVisual;
    [SerializeField] private float minFillAmount = 0;
    [SerializeField] private float maxFillAmount = 0.065f;
    [SerializeField] private float maxSize = 0.05f;
    
    private float fillAmount = 0f;
    private bool isFilled = false;
    private float initialSize = 0f;

    private int dispenserIndex = -1;
    private DispenserManager dispenserManager;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void Start()
    {
        initialSize = liquidVisual.localScale.y;
    }

    public void InitDispenserReference(DispenserManager manager, int index)
    {
        dispenserManager = manager;
        dispenserIndex = index;
    }

    public void UpdateFill(float amount)
    {
        fillAmount = amount;

        Vector3 pos = liquidVisual.localPosition;
        pos.y = Mathf.Lerp(minFillAmount, maxFillAmount, fillAmount);
        liquidVisual.localPosition = pos;

        var scale = liquidVisual.localScale;
        scale.y = Mathf.Lerp(initialSize, maxSize, fillAmount);
        liquidVisual.localScale = scale;

        if (fillAmount >= 1f && !isFilled)
        {
            isFilled = true;

            grabInteractable.enabled = true;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (dispenserIndex >= 0)
            dispenserManager.OnCupGrabbed(dispenserIndex);
    }
}