using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float searchDelay = 1f;

    public Transform headTransform;
    public LayerMask targetLayer;

    [Range(0f, 360f)]
    public float viewAngle = 120f;
    public float viewDistance = 10f;

    [SerializeField]
    private Transform nearestTarget;
    public Transform NearestTarget => nearestTarget;
    
    private float distanceToTarget = 0f;

    private Coroutine _findTarget;

    private void OnEnable()
    {
        nearestTarget = null;
        _findTarget = StartCoroutine(StartFindTarget());
    }

    private void OnDisable()
    {
        StopCoroutine(_findTarget);
    }

    private IEnumerator StartFindTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(searchDelay);
            FindTarget();
        }
    }

    private void FindTarget()
    {
        nearestTarget = null;
        distanceToTarget = 0f;

        Collider[] targets = Physics.OverlapSphere(headTransform.position, viewDistance, targetLayer);
        foreach (Collider target in targets)
        {
            Transform targetTransform = target.transform;

            Vector3 dirToTarget = ((targetTransform.position + Vector3.up) - headTransform.position).normalized;
            if (Vector3.Angle(headTransform.forward, dirToTarget) < viewAngle / 2)
            {
                float distance = Vector3.Distance(headTransform.position, targetTransform.position + Vector3.up);
                if (nearestTarget == null || distance < distanceToTarget)
                {
                    nearestTarget = targetTransform;
                    distanceToTarget = distance;
                }
            }
        }
    }
}
