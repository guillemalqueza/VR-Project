using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FoodItem : MonoBehaviour
{
    private Kitchen currentKitchen;
    private XRGrabInteractable grabInteractable;
    private bool isBeingHeld = false;
    
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }
    }
    
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isBeingHeld = true;
        
        if (currentKitchen != null)
        {
            currentKitchen.OnFoodItemSelected(args);
            currentKitchen = null;
        }
        
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null) Destroy(joint);
    }
    
    private void OnSelectExited(SelectExitEventArgs args)
    {
        isBeingHeld = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Kitchen kitchen = collision.gameObject.GetComponent<Kitchen>();
        if (kitchen != null) currentKitchen = kitchen;
    }
    
    public bool IsBeingHeld() => isBeingHeld;
    
    public bool IsInCookingProcess() => currentKitchen != null;
}
