using System;
using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;

    private Coroutine moveCoroutine;

    public void MoveTo(Vector3 destination, Action onArrive = null)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToPosition(destination, false, onArrive));
    }

    public void MoveToAndDestroy(Vector3 destination)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToPosition(destination, true, null));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, bool destroyOnArrival, Action onArrive)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        onArrive?.Invoke();

        if (destroyOnArrival)
            Destroy(gameObject);
    }
}