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

    [Header("Fill Particle Effects")]
    [SerializeField] private ParticleSystem[] fillParticles = new ParticleSystem[3];
    [SerializeField] private float waitingTime = 0.2f;

    void Start()
    {
        if (fillParticles != null)
        {
            for (int i = 0; i < fillParticles.Length; i++)
            {
                if (fillParticles[i] != null)
                {
                    fillParticles[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
        /*foreach (Dispenser dispenser in dispensers)
        {
            SpawnCup(dispenser);
            StartFilling(dispenser);
        }*/
    }

    void Update()
    {
        for (int i = 0; i < dispensers.Length; i++)
        {
            Dispenser dispenser = dispensers[i];
            if (dispenser.isFilling && dispenser.currentCup != null)
            {
                dispenser.fillAmount += Time.deltaTime / dispenser.fillTime;

                if (fillParticles != null && i >= 0 && i < fillParticles.Length && fillParticles[i] != null)
                {
                    if (dispenser.fillAmount >= 0.95f && (fillParticles[i].isEmitting || fillParticles[i].isPlaying))
                    {
                        fillParticles[i].Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    }
                }

                if (dispenser.fillAmount >= 1f)
                    CompleteFill(dispenser, i);

                UpdateCupFill(dispenser);
            }
        }
    }

    private void StartFilling(Dispenser dispenser)
    {
        int index = System.Array.IndexOf(dispensers, dispenser);
        if (dispenser.currentCup != null && !dispenser.isFilling && dispenser.fillAmount < 1f)
        {
            if (fillParticles != null && index >= 0 && index < fillParticles.Length && fillParticles[index] != null)
            {
                if (!fillParticles[index].isPlaying)
                    fillParticles[index].Play();
            }

            StartCoroutine(StartFillingDelayed(dispenser, waitingTime));
        }
    }

    private IEnumerator StartFillingDelayed(Dispenser dispenser, float delay)
    {
        yield return new WaitForSeconds(delay);
        dispenser.isFilling = true;
    }

    private void CompleteFill(Dispenser dispenser, int index)
    {
        dispenser.isFilling = false;
        dispenser.fillAmount = 1f;
        if (fillParticles != null && index >= 0 && index < fillParticles.Length && fillParticles[index] != null)
        {
            if (fillParticles[index].isEmitting || fillParticles[index].isPlaying)
                fillParticles[index].Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
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
        GameObject cupObj = Instantiate(dispenser.orderItemSO.itemPrefab, dispenser.cupSpawnTransform.position, Quaternion.identity);
        dispenser.currentCup = cupObj;
        dispenser.isFilling = false;
        dispenser.fillAmount = 0f;

        Cup cup = cupObj.GetComponent<Cup>();
        cup.InitDispenserReference(this, System.Array.IndexOf(dispensers, dispenser));
    }
    
    public void TrySpawnCup(int index)
    {
        if (index < 0 || index >= dispensers.Length) return;

        Dispenser dispenser = dispensers[index];

        if (dispenser.currentCup == null)
        {
            SpawnCup(dispenser);
            StartFilling(dispenser);
        }
    }
    
    public void OnCupGrabbed(int dispenserIndex)
    {
        if (dispenserIndex >= 0 && dispenserIndex < dispensers.Length)
        {
            dispensers[dispenserIndex].currentCup = null;
            dispensers[dispenserIndex].isFilling = false;
            dispensers[dispenserIndex].fillAmount = 0f;
        }
    }
}