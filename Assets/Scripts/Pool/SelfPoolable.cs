using Pooling.Poolers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Poolable))]
public class SelfPoolable : MonoBehaviour
{
    public Poolable poolable;
    [Tooltip("Same pool the asset was Dequeue from")]
    public SetPooler pool;
    IEnumerator currentRutine;

    public bool startWaitTimeOnEnable = false;
    public float waitTime = 1;

    private void OnValidate()
    {
        if (!poolable)
            poolable = GetComponent<Poolable>();
    }
    private void OnEnable()
    {
        if (startWaitTimeOnEnable && pool != null)
            WaitAndPool(waitTime);
    }

    public void Enqueue()
    {
        pool.Enqueue(poolable);
    }

    public void WaitAndPool(float waitTime) => WaitAndPool(waitTime, pool);
    public void WaitAndPool(float waitTime, SetPooler pool)
    {
        this.pool = pool;
        if (currentRutine != null) StopCoroutine(currentRutine);
        currentRutine = WaitAndPoolRoutine(waitTime);
        StartCoroutine(currentRutine);
    }

    IEnumerator WaitAndPoolRoutine(float t )
    {
        yield return new WaitForSeconds(t);
        Enqueue();
    }

    private void OnDisable()
    {
        if (currentRutine != null) StopCoroutine(currentRutine);
    }

    private void OnDestroy()
    {
        if(pool != null)
            pool.Remove(poolable);
    }
}

public static class SelfPoolableGameobjectExtension
{
    public static void ClearSelfPoolables(this GameObject go)
    {
        SelfPoolable[] poolables = go.GetComponentsInChildren<SelfPoolable>();
        foreach (SelfPoolable item in poolables)
        {
            item.Enqueue();
        }
    }
}