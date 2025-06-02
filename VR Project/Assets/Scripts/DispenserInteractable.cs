using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DispenserInteractable : XRBaseInteractable
{
    [SerializeField] private int dispenserIndex;
    [SerializeField] private DispenserManager dispenserManager;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        dispenserManager.TrySpawnCup(dispenserIndex);
    }
}
