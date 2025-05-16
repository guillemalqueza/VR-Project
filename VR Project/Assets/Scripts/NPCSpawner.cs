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
        CustomerOrder order = currentCustomer.GetComponent<CustomerOrder>();

        movement.MoveTo(orderPoint.position, () =>
        {
            order?.RequestOrder();
        });
    }

    public void SendCustomerToExit()
    {
        if (currentCustomer == null) return;

        CustomerOrder order = currentCustomer.GetComponent<CustomerOrder>();
        order?.CompleteOrder();

        NPCMovement movement = currentCustomer.GetComponent<NPCMovement>();
        movement.MoveToAndDestroy(exitPoint.position);
    }
}