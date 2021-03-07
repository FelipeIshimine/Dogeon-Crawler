using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(SelfPoolable), typeof(TopDownPhysics2D))]
public class Projectile : MonoBehaviour
{
    public TopDownPhysics2D topDownPhysics2D;
    public float speed = 10;

    public float lifeTime = 3;
    
    [SerializeField] private SelfPoolable selfpoolable;

    public SelfPoolable SelfPoolable => selfpoolable;
    
    private void OnValidate()
    {
        if (!selfpoolable)
            selfpoolable = GetComponent<SelfPoolable>();
        if (!topDownPhysics2D)
            topDownPhysics2D = GetComponent<TopDownPhysics2D>();
    }

    [Button]
    public void Fire(Vector2 direction) => topDownPhysics2D.AddForce(direction * speed);
    
    public void Fire()=> topDownPhysics2D.AddForce(transform.up * speed);

}
