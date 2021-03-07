using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageOnContact : DamageOn
{

    public void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision);
    }
    
    public void OnCollisionStay2D(Collision2D collision)
    {
        ProcessCollision(collision);
    }

    private void ProcessCollision(Collision2D collision)
    {
        if (!targetMask.Contains(collision.gameObject.layer)) return;
        
        var targetHealth = collision.gameObject.GetComponent<Health>();
        if (!targetHealth || !targetHealth.IsAlive) return;
        ApplyDamage(targetHealth, collision);
    }
}