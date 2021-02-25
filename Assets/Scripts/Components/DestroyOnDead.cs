using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyOnDead : MonoBehaviour
{
    public Health health;

    private void Awake()
    {
        health.OnDead += DestroySelf;
    }

    private void DestroySelf(Health h)
    {
        health.OnDead -= DestroySelf;
        Destroy(gameObject);
    }
}
