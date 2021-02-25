using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackOnDamage : MonoBehaviour
{
    public TopDownPhysics2D topDownPhysics;
    public Health health;

    public float knockBackForce = 10;

    private void OnValidate()
    {
        if (!topDownPhysics) GetComponent<TopDownPhysics2D>();
        if (!health) GetComponent<Health>();
    }

    private void Awake()
    {
        health.OnModifyWithSource += OnHealthModify;
    }
    private void OnDestroy()
    {
        health.OnModifyWithSource -= OnHealthModify;
    }

    private void OnHealthModify(int old, int current, object source)
    {
        if(old > current)
        {
            Vector2 direction = Vector2.zero;
            
            if (source is Collision2D collision2D)
                direction = -collision2D.contacts[0].normal;
            else if (source is MonoBehaviour monoBehaviour)
              direction =  monoBehaviour.transform.position.GetDirection(transform.position);

            topDownPhysics.AddForce(direction * knockBackForce);
        }
    }


}
