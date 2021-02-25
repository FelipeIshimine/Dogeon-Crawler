using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public Action<int,Health> OnDamageDone;
    public Action<Health> OnKill;
    public LayerMask targetMask;
    public int damage =1;

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(targetMask.Contains(collision.gameObject.layer))
        {
            var targetHealth = collision.gameObject.GetComponent<Health>();
            if (!targetHealth || !targetHealth.IsAlive) return;
            
            OnDamageDone?.Invoke(damage, targetHealth);
            if (targetHealth.Damage(damage, collision))
                OnKill?.Invoke(targetHealth);
        }
    }
}