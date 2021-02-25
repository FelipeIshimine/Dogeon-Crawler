using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action<int, int, object> OnModifyWithSource;
    public Action<int, int> OnModify;
    public Action<Health> OnDead;

    public bool canDie = true;

    public int maxHealth = 1;
    [ShowInInspector] public int Current { get; private set; }

    public bool IsAlive => Current > 0;


    public float invulnerabilityTimeCD = 1;
    public float invulnerabilityTimer;
    public bool IsInvulnerable => invulnerabilityTimer > 0;

    public void FixedUpdate()
    {
        if (IsInvulnerable)
            invulnerabilityTimer -= Time.fixedDeltaTime;
    }
    private void Awake()
    {
        Current = maxHealth;    
    }

    public bool Damage(int amount, object source)
    {
        if (!IsAlive)
            return false;

        Modify(-amount, source);
        return !IsAlive;
    }

    public void Modify(int amount, object source)
    {
        if (!IsAlive) return;

        if (amount < 0)
        {
            if(IsInvulnerable) 
                return;
            invulnerabilityTimer = invulnerabilityTimeCD;
        }

        int old = Current;
        Current += amount;

        if (!canDie && Current <= 0)
            Current = 1;

        if (Current < 0) Current = 0;

        OnModify?.Invoke(old, Current);
        OnModifyWithSource?.Invoke(old, Current, source);
        if (Current == 0)
            OnDead?.Invoke(this);
    }
}
