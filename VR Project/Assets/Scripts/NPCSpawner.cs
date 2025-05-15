using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] customerPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform orderPoint;
    [SerializeField] private Transform exitPoint;

    private GameObject currentCustomer;

    void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        if (customerPrefabs.Length == 0) return;

        GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        currentCustomer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        NPCMovement movement = currentCustomer.GetComponent<NPCMovement>();
        movement.MoveTo(orderPoint.position);
    }

    public void SendCustomerToExit()
    {
        if (currentCustomer == null) return;

        NPCMovement movement = currentCustomer.GetComponent<NPCMovement>();
        movement.MoveToAndDestroy(exitPoint.position);
    }
}