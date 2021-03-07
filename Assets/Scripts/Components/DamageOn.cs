using System;
using UnityEngine;

public abstract class DamageOn : MonoBehaviour
{
    public Action<int,Health> OnDamageDone;
    public Action<Health> OnKill;
    public LayerMask targetMask;
    public int damage =1;

    protected void ApplyDamage(Health targetHealth, object source)
    {
        if (!targetHealth || !targetHealth.IsAlive) return;

        OnDamageDone?.Invoke(damage, targetHealth);
        if (targetHealth.Damage(damage, source))
            OnKill?.Invoke(targetHealth);
    }
}