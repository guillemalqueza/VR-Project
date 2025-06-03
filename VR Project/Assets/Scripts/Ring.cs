using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class Ring : XRBaseInteractable
{
    [SerializeField] private CheckOrders checkOrders;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float waitTime = 2f;

    private bool isWaiting = false;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isWaiting)
            StartCoroutine(RingActionCoroutine());
    }

    private IEnumerator RingActionCoroutine()
    {
        isWaiting = true;
        checkOrders.CheckOrder();
        audioSource.Play();
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
