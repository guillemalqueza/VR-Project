using System.Collections;
using UnityEngine;

public class DispenserManager : MonoBehaviour
{
    [System.Serializable]
    public class Dispenser
    {
        [SerializeField] public Transform cupSpawnTransform;
        [SerializeField] public float fillTime = 3f;
        [SerializeField] public OrderItemSO orderItemSO;

        [HideInInspector] public GameObject currentCup;
        [HideInInspector] public bool isFilling = false;
        [HideInInspector] public float fillAmount = 0f;
    }

    [Header("Dispensers Configuration")]
    [SerializeField] private Dispenser[] dispensers = new Dispenser[3];
    
    [Header("Cup Settings")]
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private float pickupDistance = 1.5f;

    void Start()
    {
        foreach (Dispenser dispenser in dispensers)
        {
            SpawnCup(dispenser);
            StartFilling(dispenser);
        }
    }

    void Update()
    {
        foreach (Dispenser dispenser in dispensers)
        {
            if (dispenser.isFilling && dispenser.currentCup != null)
            {
                dispenser.fillAmount += Time.deltaTime / dispenser.fillTime;
                
                if (dispenser.fillAmount >= 1f)
                    CompleteFill(dispenser);

                UpdateCupFill(dispenser);
            }
        }
    }

    private void StartFilling(Dispenser dispenser)
    {
        if (dispenser.currentCup != null && !dispenser.isFilling && dispenser.fillAmount < 1f)
        {
            dispenser.isFilling = true;
        }
    }

    private void CompleteFill(Dispenser dispenser)
    {
        dispenser.isFilling = false;
        dispenser.fillAmount = 1f;
    }

    private void UpdateCupFill(Dispenser dispenser)
    {
        Cup cupComponent = dispenser.currentCup.GetComponent<Cup>();
        cupComponent.UpdateFill(dispenser.fillAmount);
    }

    private IEnumerator RespawnCupAfterDelay(Dispenser dispenser)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnCup(dispenser);
    }

    private void SpawnCup(Dispenser dispenser)
    {
        dispenser.currentCup = Instantiate(dispenser.orderItemSO.itemPrefab, dispenser.cupSpawnTransform.position, Quaternion.identity);
        dispenser.isFilling = false;
        dispenser.fillAmount = 0f;
    }
}