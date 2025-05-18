using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class IngredientSpawner : XRBaseInteractable
{
    public event EventHandler OnIngredientSpawned;
    
    [SerializeField] private OrderItemSO ingredientSO;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnForce = 0f;
    [SerializeField] private bool allowRayInteractors = true;
    [SerializeField] private bool acceptAllInteractors = true;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(SpawnIngredient);
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(SpawnIngredient);
    }
    
    private void SpawnIngredient(SelectEnterEventArgs args)
    {
        if (ingredientSO == null)
        {
            Debug.LogError("IngredientSO not assigned to the spawner!", this);
            return;
        }
        
        var interactor = args.interactorObject;

        string interactorInfo = $"Interactor: {interactor.GetType().Name}";
        if (interactor is Component comp)
        {
            interactorInfo += $" (GameObject: {comp.gameObject.name})";

            Transform parent = comp.transform.parent;
            string hierarchy = "";
            while (parent != null)
            {
                hierarchy = $"{parent.gameObject.name}/" + hierarchy;
                parent = parent.parent;
            }
            
            if (!string.IsNullOrEmpty(hierarchy))
            {
                interactorInfo += $" in hierarchy: {hierarchy}{comp.gameObject.name}";
            }
        }
        Debug.Log(interactorInfo, this);

        bool isValidInteractor = interactor is XRDirectInteractor;

        if (!isValidInteractor && allowRayInteractors)
        {
            isValidInteractor = interactor is XRRayInteractor;
        }

        if (!isValidInteractor && acceptAllInteractors)
        {
            isValidInteractor = interactor is IXRSelectInteractor;
        }
        
        if (!isValidInteractor)
        {
            Debug.LogWarning($"Spawner was interacted with by {interactor.GetType().Name}. Expected XRDirectInteractor or XRRayInteractor.", this);
            return;
        }

        var spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;

        if (interactor is XRRayInteractor rayInteractor)
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out var raycastHit))
            {
                spawnPosition = raycastHit.point;
            }
            else
            {
                spawnPosition = rayInteractor.transform.position + rayInteractor.transform.forward * 0.2f;
            }
        }

        GameObject ingredientObject = Instantiate(ingredientSO.itemPrefab, spawnPosition, Quaternion.identity);
        XRGrabInteractable grabInteractable = ingredientObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = ingredientObject.AddComponent<XRGrabInteractable>();

            grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
            grabInteractable.throwOnDetach = true;

            if (!ingredientObject.GetComponent<Rigidbody>())
            {
                Rigidbody rb = ingredientObject.AddComponent<Rigidbody>();
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }

        StartCoroutine(GrabNextFrame(interactor, grabInteractable));

        OnIngredientSpawned?.Invoke(this, EventArgs.Empty);
    }
    
    private System.Collections.IEnumerator GrabNextFrame(IXRInteractor interactor, XRGrabInteractable grabInteractable)
    {
        yield return null;
        
        if (interactor is IXRSelectInteractor selectInteractor && 
            selectInteractor.CanSelect((IXRSelectInteractable)grabInteractable) && 
            !grabInteractable.isSelected)
        {
            if (interactionManager != null)
            {
                interactionManager.SelectEnter(selectInteractor, (IXRSelectInteractable)grabInteractable);
            }
            else
            {
                Debug.LogWarning("No XRInteractionManager found for IngredientSpawner.", this);
            }
        }
        else
        {
            Rigidbody rb = grabInteractable.GetComponent<Rigidbody>();
            if (rb != null && interactor is Component interactorComponent)
            {
                Vector3 direction = (interactorComponent.transform.position - grabInteractable.transform.position).normalized;
                rb.AddForce(direction * spawnForce, ForceMode.Impulse);
            }
        }
    }
    public OrderItemSO GetIngredientSO()
    {
        return ingredientSO;
    }
}
