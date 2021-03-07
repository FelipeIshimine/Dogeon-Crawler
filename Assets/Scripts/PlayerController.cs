using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Weapon
    {
        Sword,
        Bow
    }

    public Weapon weapon = Weapon.Bow;
    
    public TopDownPhysics2D platformerPhysics;
    public Health health;
    public Bow bow;
    public Sword sword;
    
    public float speed = 2;
    public float dashForce = 30;

    public bool useRaw = true;
    public Vector2 input;

    public float dashCooldown = 3;

    public float damageStunCD = .5f;

    public float damageStunTimer;
    private bool IsStunByDamage => damageStunTimer > 0;
    private bool IsStunByAttack => sword && sword.IsAttacking;

    private bool IsDashReady => dashTimer <= 0;
    [ShowInInspector,ReadOnly]private float dashTimer;
    private Camera cam;


    private void Start()
    {
        cam = Camera.main;
    }

    private void Awake()
    {
        health.OnModify += OnModify;   
        Swap();
    }

    public void Swap()
    {
        switch (weapon)
        {
            case Weapon.Sword:
                weapon = Weapon.Bow;
                bow.gameObject.SetActive(true);
                sword.gameObject.SetActive(false);
                break;
            case Weapon.Bow:
                weapon = Weapon.Sword;
                bow.gameObject.SetActive(false);
                sword.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
            damageStunTimer -= Time.fixedDeltaTime;

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            Swap();
        
        if(IsStunByDamage) return;

        if (Input.GetKey(KeyCode.LeftShift))
            Dash(input);

        if(weapon == Weapon.Sword && Input.GetButtonDown("Fire1"))
            sword.Fire();
        
        if (IsStunByAttack) return;

        transform.rotation = Quaternion.Euler(0, 0,
            transform.position.GetDirection(cam.ScreenToWorldPoint(Input.mousePosition)).AsAngle2D()-90);
        if (weapon == Weapon.Bow)
        {
            //bow.Aim(transform.position.GetDirection(cam.ScreenToWorldPoint(Input.mousePosition)));
            
            if(Input.GetButtonDown("Fire2"))
                bow.Fire();
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
    }

    public void Dash(Vector2 direction)
    {
        if (!IsDashReady) 
            return;

        platformerPhysics.AddForce(direction * dashForce);
        dashTimer = dashCooldown;
    }
}
