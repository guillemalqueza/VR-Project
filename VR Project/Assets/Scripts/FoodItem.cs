using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FoodItem : MonoBehaviour
{
    private Kitchen currentKitchen;
    private XRGrabInteractable grabInteractable;
    private bool isBeingHeld = false;
    [SerializeField] private GameObject[] stateGameObjects;
    
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
    
    public void ChangeState(Kitchen.State state)
    {
        if (stateGameObjects == null || stateGameObjects.Length < 3)
        {
            return;
        }
        
        switch (state)
        {
            case Kitchen.State.Fried:
                if (stateGameObjects[0] != null)
                {
                    foreach (Renderer renderer in stateGameObjects[0].GetComponentsInChildren<Renderer>())
                    {
                        renderer.enabled = false;
                    }
                    
                    if (grabInteractable != null && stateGameObjects[0].GetComponent<Collider>() != null && 
                        stateGameObjects[0] == grabInteractable.GetComponent<Collider>()?.gameObject)
                    {
                        stateGameObjects[0].GetComponent<Collider>().enabled = true;
                    }
                }
                
                if (stateGameObjects[1] != null)
                {
                    stateGameObjects[1].SetActive(true);
                }
                break;

            case Kitchen.State.Burned:
                if (stateGameObjects[1] != null)
                {
                    foreach (Renderer renderer in stateGameObjects[1].GetComponentsInChildren<Renderer>())
                    {
                        renderer.enabled = false;
                    }
                    
                    if (grabInteractable != null && stateGameObjects[1].GetComponent<Collider>() != null && 
                        stateGameObjects[1] == grabInteractable.GetComponent<Collider>()?.gameObject)
                    {
                        stateGameObjects[1].GetComponent<Collider>().enabled = true;
                    }
                }
                
                if (stateGameObjects[2] != null)
                {
                    stateGameObjects[2].SetActive(true);
                }
                break;
        }
    }

    public bool IsBeingHeld() => isBeingHeld;
    
    public bool IsInCookingProcess() => currentKitchen != null;
}
