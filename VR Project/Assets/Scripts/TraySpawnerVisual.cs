using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySpawnerVisual : MonoBehaviour
{
    [SerializeField] private TraySpawner traySpawner;
    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform trayVisualPrefab;
    [SerializeField] private float trayOffsetY = 0.05f;

    private List<GameObject> trayVisualGameObjectList;

    private void Awake()
    {
        trayVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        traySpawner.OnTraySpawned += TraySpawner_OnTraySpawned;
        traySpawner.OnTrayRemoved += TraySpawner_OnTrayRemoved;
    }

    private void TraySpawner_OnTrayRemoved(object sender, System.EventArgs e)
    {
        GameObject trayGameObject = trayVisualGameObjectList[trayVisualGameObjectList.Count - 1];
        trayVisualGameObjectList.Remove(trayGameObject);
        Destroy(trayGameObject);
    }

    private void TraySpawner_OnTraySpawned(object sender, System.EventArgs e)
    {
        Transform trayVisualTransform = Instantiate(trayVisualPrefab, topPoint);

        trayVisualTransform.localPosition = new Vector3(0, trayOffsetY * trayVisualGameObjectList.Count, 0);

        trayVisualGameObjectList.Add(trayVisualTransform.gameObject);
    }
}
