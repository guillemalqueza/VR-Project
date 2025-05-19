using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Kitchen : MonoBehaviour, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs { public State currentState; }
    
    public enum State { Idle, Frying, Fried, Burned }
    
    [SerializeField] private Transform[] topPoints;
    [SerializeField] private bool isStove;
    
    private List<GameObject> currentFoodItems = new List<GameObject>();
    private List<float> fryingTimers = new List<float>();
    private List<float> burningTimers = new List<float>();
    private List<State> foodStates = new List<State>();
    
    private float burningTimerMax = 5f;
    private float fryingTimerMax = 10f;
    private State currentState;

    private void Start()
    {
        currentState = State.Idle;
        
        for (int i = 0; i < topPoints.Length; i++)
        {
            currentFoodItems.Add(null);
            fryingTimers.Add(0f);
            burningTimers.Add(0f);
            foodStates.Add(State.Idle);
        }
    }
    
    private void Update()
    {
        for (int i = 0; i < topPoints.Length; i++)
        {
            if (currentFoodItems[i] == null) continue;
            
            if (foodStates[i] == State.Frying)
            {
                // Mantener comida en posición mientras se cocina
                currentFoodItems[i].transform.position = topPoints[i].position;
                currentFoodItems[i].transform.rotation = topPoints[i].rotation;
                
                fryingTimers[i] += Time.deltaTime;
                float fryingProgress = fryingTimers[i] / fryingTimerMax;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = fryingProgress });
                
                if (fryingTimers[i] >= fryingTimerMax)
                {
                    fryingTimers[i] = 0f;
                    foodStates[i] = State.Fried;
                    MakeFoodInteractable(i);
                    UpdateOverallState();
                    Debug.Log((isStove ? "Hamburger" : "Potatoes") + " at position " + i + " is cooked!");
                }
            }
            else if (foodStates[i] == State.Fried)
            {
                burningTimers[i] += Time.deltaTime;
                float burningProgress = burningTimers[i] / burningTimerMax;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = burningProgress });
                
                if (burningTimers[i] >= burningTimerMax)
                {
                    burningTimers[i] = 0f;
                    foodStates[i] = State.Burned;
                    UpdateOverallState();
                    Debug.Log((isStove ? "Hamburger" : "Potatoes") + " at position " + i + " is burned!");
                }
            }
        }
    }

    private void MakeFoodInteractable(int index)
    {
        if (currentFoodItems[index] == null) return;
        
        Vector3 currentPosition = currentFoodItems[index].transform.position;
        Quaternion currentRotation = currentFoodItems[index].transform.rotation;
        
        Rigidbody foodRb = currentFoodItems[index].GetComponent<Rigidbody>();
        if (foodRb != null) foodRb.isKinematic = false;
        
        var grabInteractable = currentFoodItems[index].GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
            grabInteractable.throwOnDetach = true;
        }
        
        if (!currentFoodItems[index].TryGetComponent<FixedJoint>(out var joint))
        {
            joint = currentFoodItems[index].AddComponent<FixedJoint>();
            joint.connectedBody = GetComponent<Rigidbody>();
            joint.breakForce = 2000f;
            joint.breakTorque = 2000f;
        }
        
        currentFoodItems[index].transform.position = currentPosition;
        currentFoodItems[index].transform.rotation = currentRotation;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        bool isValidItem = (isStove && collision.gameObject.CompareTag("Burger")) || (!isStove && collision.gameObject.CompareTag("Potato"));
        
        if (!isValidItem) return;
        
        FoodItem foodItem = collision.gameObject.GetComponent<FoodItem>();
        if (foodItem != null && foodItem.IsBeingHeld()) return;
        
        int emptyIndex = -1;
        for (int i = 0; i < topPoints.Length; i++)
        {
            if (currentFoodItems[i] == null)
            {
                emptyIndex = i;
                break;
            }
        }
        
        if (emptyIndex >= 0) PlaceFoodAtIndex(collision.gameObject, emptyIndex);
    }

    private void PlaceFoodAtIndex(GameObject foodObject, int index)
    {
        Rigidbody foodRb = foodObject.GetComponent<Rigidbody>();
        if (foodRb != null) foodRb.isKinematic = true;
        
        var grabInteractable = foodObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null) grabInteractable.enabled = false;
        
        currentFoodItems[index] = foodObject;
        currentFoodItems[index].transform.position = topPoints[index].position;
        currentFoodItems[index].transform.rotation = topPoints[index].rotation;
        
        foodStates[index] = State.Frying;
        UpdateOverallState();
    }
    
    private void UpdateOverallState()
    {
        bool hasFood = false;
        bool allBurned = true;
        bool hasFried = false;
        bool hasFrying = false;
        
        for (int i = 0; i < topPoints.Length; i++)
        {
            if (currentFoodItems[i] != null)
            {
                hasFood = true;
                if (foodStates[i] != State.Burned) allBurned = false;
                if (foodStates[i] == State.Fried) hasFried = true;
                if (foodStates[i] == State.Frying) hasFrying = true;
            }
        }
        
        State newState = State.Idle;
        if (hasFood)
        {
            if (allBurned) newState = State.Burned;
            else if (hasFried) newState = State.Fried;
            else if (hasFrying) newState = State.Frying;
        }
        
        currentState = newState;
        for (int i = 0; i < topPoints.Length; i++)
        {
            if (currentFoodItems[i] != null)
            {
                FoodItem foodItem = currentFoodItems[i]?.GetComponent<FoodItem>();
                if (foodItem != null)
                {
                    foodItem.ChangeState(currentState);
                }
            }
        }
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { currentState = currentState });
    }
    
    public bool HasFood() => currentFoodItems.Exists(item => item != null);
    
    public State GetState() => currentState;
    
    public GameObject GetFoodItem(int index) => (index >= 0 && index < currentFoodItems.Count) ? currentFoodItems[index] : null;
    
    public GameObject GetFoodItem()
    {
        for (int i = 0; i < currentFoodItems.Count; i++)
            if (currentFoodItems[i] != null) return currentFoodItems[i];
        return null;
    }
    
    public void RemoveFoodItem(int index)
    {
        if (index < 0 || index >= currentFoodItems.Count || currentFoodItems[index] == null) return;
        
        Rigidbody foodRb = currentFoodItems[index].GetComponent<Rigidbody>();
        if (foodRb != null) foodRb.isKinematic = false;
        
        var grabInteractable = currentFoodItems[index].GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && !grabInteractable.enabled) grabInteractable.enabled = true;
        
        FixedJoint joint = currentFoodItems[index].GetComponent<FixedJoint>();
        if (joint != null) Destroy(joint);
        
        currentFoodItems[index] = null;
        fryingTimers[index] = 0f;
        burningTimers[index] = 0f;
        foodStates[index] = State.Idle;
        
        UpdateOverallState();
    }
    
    public void OnFoodItemSelected(SelectEnterEventArgs args)
    {
        GameObject selectedObject = args.interactableObject.transform.gameObject;
        
        for (int i = 0; i < currentFoodItems.Count; i++)
        {
            if (currentFoodItems[i] == selectedObject)
            {
                if (CanGrabFoodItem(i)) RemoveFoodItem(i);
                break;
            }
        }
    }

    public void ClearKitchen()
    {
        for (int i = 0; i < topPoints.Length; i++)
        {
            currentFoodItems[i] = null;
            fryingTimers[i] = 0f;
            burningTimers[i] = 0f;
            foodStates[i] = State.Idle;
        }
        
        currentState = State.Idle;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progress = 0f });
    }
    
    public bool CanGrabFoodItem(int index)
    {
        if (index >= 0 && index < foodStates.Count && currentFoodItems[index] != null)
            return foodStates[index] == State.Fried || foodStates[index] == State.Burned;
        return false;
    }
}
