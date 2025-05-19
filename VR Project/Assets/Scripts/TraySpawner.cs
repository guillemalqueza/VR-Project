using System;
using UnityEngine;

public class TraySpawner : MonoBehaviour
{
    public event EventHandler OnTraySpawned;
    public event EventHandler OnTrayRemoved;

    private float spawnTrayTimer;
    [SerializeField] private float spawnTrayTimerMax = 5f;
    private int trayCount;
    [SerializeField] private int trayCountMax = 3;

    private void Update()
    {
        if (trayCount >= trayCountMax)
            return;

        spawnTrayTimer += Time.deltaTime;
        if (spawnTrayTimer >= spawnTrayTimerMax)
        {
            spawnTrayTimer = 0f;
            SpawnTray();
        }
    }

    public void SpawnTray()
    {
        if (trayCount < trayCountMax)
        {
            trayCount++;
            OnTraySpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveTray()
    {
        trayCount--;
        OnTrayRemoved.Invoke(this, EventArgs.Empty);
    }
}
