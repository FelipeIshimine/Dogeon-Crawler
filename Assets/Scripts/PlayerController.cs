using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TopDownPhysics2D platformerPhysics;
    public Health health;

    public float speed = 2;
    public float dashForce = 30;

    public bool useRaw = true;
    public Vector2 input;

    public float dashCooldown = 3;


    public float damageStunCD = .5f;

    public float damageStunTimer;
    private bool IsStunByDamage => damageStunTimer > 0;

    private bool IsDashReady => dashTimer <= 0;
    [ShowInInspector,ReadOnly]private float dashTimer;


    private void Awake()
    {
        health.OnModify += OnModify;   
    }
    private void OnDestroy()
    {
        health.OnModify -= OnModify;
    }

    private void OnModify(int old, int current)
    {
        if(old > current)//Damage
            damageStunTimer = damageStunCD;
    }

    private void FixedUpdate()
    {
        if (!IsDashReady)
            dashTimer -= Time.fixedDeltaTime;

        if (IsStunByDamage)
        {
            damageStunTimer -= Time.fixedDeltaTime;
            return;
        }


        if (useRaw)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
        }

        if(Mathf.Abs(platformerPhysics.Velocity.magnitude) < Mathf.Abs((input*speed).magnitude))
            platformerPhysics.SetVelocity(speed * input.normalized);

        if (Input.GetKey(KeyCode.LeftShift))
            Dash(input);
    }

    public void Dash(Vector2 direction)
    {
        if (!IsDashReady) 
            return;

        platformerPhysics.AddForce(direction * dashForce);
        dashTimer = dashCooldown;
    }
}
