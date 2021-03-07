using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DamageOn),typeof(Animator))]
public class Sword : MonoBehaviour
{
    public Animator animator;
    public DamageOn damageOn;
    public TopDownPhysics2D physics2D;
    
    [ShowInInspector, ReadOnly] private int comboIndex;
    [ShowInInspector, ReadOnly] private int maxComboIndex = 3;
    private static readonly int ComboIndex = Animator.StringToHash("ComboIndex");

    public Vector2[] extraVelocity;

    public float resetCooldown = 1.75f;
    private float resetTimer;
    
    public bool IsReady { get; private set; } = true;
    public bool IsAttacking { get; set; } = false;

    public void SetReady() => IsReady = true;

    private void OnValidate()
    {
        if (!damageOn) damageOn = GetComponent<DamageOn>();
        if (!animator) animator = GetComponent<Animator>();
    }

    public void Fire()
    {
        if (!IsReady || comboIndex == maxComboIndex) return;

        IsAttacking = true;
        if (physics2D && comboIndex < extraVelocity.Length)
            physics2D.SetVelocity(Quaternion.Euler(0, 0, extraVelocity[comboIndex].AsAngle()-90)  * extraVelocity[comboIndex]);
      
        comboIndex++;
        animator.SetInteger(ComboIndex, comboIndex);
    }

    public void ResetIndex()
    {
        comboIndex = 0;
        animator.SetInteger(ComboIndex, comboIndex);
    }
    
    public void Update()
    {
        if (resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
        }
    }
}
