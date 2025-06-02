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
    [SerializeField] private AudioSource fillingAudioSource;
    
    private float fillAmount = 0f;
    private bool isFilled = false;
    private float initialSize = 0f;

    private int dispenserIndex = -1;
    private DispenserManager dispenserManager;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private bool wasFilling = false;

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
        
        if (liquidVisual != null)
            liquidVisual.gameObject.SetActive(false);
    }

    public void InitDispenserReference(DispenserManager manager, int index)
    {
        dispenserManager = manager;
        dispenserIndex = index;
    }

    public void UpdateFill(float amount)
    {
        fillAmount = amount;

        if (fillAmount > 0f && !liquidVisual.gameObject.activeSelf)
            liquidVisual.gameObject.SetActive(true);

        Vector3 pos = liquidVisual.localPosition;
        pos.y = Mathf.Lerp(minFillAmount, maxFillAmount, fillAmount);
        liquidVisual.localPosition = pos;

        var scale = liquidVisual.localScale;
        scale.y = Mathf.Lerp(initialSize, maxSize, fillAmount);
        liquidVisual.localScale = scale;

        bool isCurrentlyFilling = fillAmount > 0f && fillAmount < 1f;
        if (isCurrentlyFilling && !wasFilling)
        {
            if (!fillingAudioSource.isPlaying)
                fillingAudioSource.Play();
        }
        else if (!isCurrentlyFilling && wasFilling)
        {
            if (fillingAudioSource.isPlaying)
                fillingAudioSource.Stop();
        }
        wasFilling = isCurrentlyFilling;

        if (fillAmount >= 1f && !isFilled)
        {
            isFilled = true;

            grabInteractable.enabled = true;
            rb.isKinematic = false;
            rb.useGravity = true;

            if (fillingAudioSource.isPlaying)
                fillingAudioSource.Stop();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (dispenserIndex >= 0)
            dispenserManager.OnCupGrabbed(dispenserIndex);
    }
}