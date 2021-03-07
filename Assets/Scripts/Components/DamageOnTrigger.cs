using UnityEngine;

public class DamageOnTrigger : DamageOn
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        ProcessCollision(collider);
    }
    
    public void OnTriggerStay2D(Collider2D collider)
    {
        ProcessCollision(collider);
    }

    private void ProcessCollision(Collider2D collider)
    {
        if (!targetMask.Contains(collider.gameObject.layer)) return;
         
        var targetHealth = collider.gameObject.GetComponent<Health>();
        ApplyDamage(targetHealth, this);
    }

  
}