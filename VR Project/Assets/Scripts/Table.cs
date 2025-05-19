using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Table : MonoBehaviour
{
    [SerializeField] private Transform[] trayPoints;
    [SerializeField] private TraySpawner traySpawner;
    
    private List<GameObject> currentTrays = new List<GameObject>();
    
    private void Start()
    {
        for (int i = 0; i < trayPoints.Length; i++)
        {
            currentTrays.Add(null);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Tray")) return;
        
        XRGrabInteractable grabInteractable = collision.gameObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            return;
        }
        FoodItem foodItem = collision.gameObject.GetComponent<FoodItem>();
        if (foodItem != null && foodItem.IsBeingHeld()) return;

        int emptyIndex = -1;
        for (int i = 0; i < trayPoints.Length; i++)
        {
            if (currentTrays[i] == null)
            {
                emptyIndex = i;
                break;
            }
        }
        
        if (emptyIndex >= 0) PlaceTrayAtIndex(collision.gameObject, emptyIndex);
    }

    private void PlaceTrayAtIndex(GameObject trayObject, int index)
    {
        Rigidbody trayRb = trayObject.GetComponent<Rigidbody>();
        if (trayRb != null)
        {
            trayRb.isKinematic = true;
            trayRb.constraints = RigidbodyConstraints.FreezeAll; 
        }

        var grabInteractable = trayObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null) grabInteractable.enabled = false;

        currentTrays[index] = trayObject;
        currentTrays[index].transform.position = trayPoints[index].position;
        currentTrays[index].transform.rotation = trayPoints[index].rotation;
        
        Debug.Log($"Placed tray at index {index}");

        traySpawner.RemoveTray();
    }
}
