using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public TopDownPhysics2D topDownPhysics2D;
    public float speed = 10;

    public float lifeTime = 3;

    public bool destroyOnContact = true;
    
    [Button]
    public void Fire(Vector2 direction)
    {
        topDownPhysics2D.AddForce(direction * speed);
    }

}
