using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : Projectile
{
    public LayerMask targetMask;

    public GameObject head;
    public Collider2D col;
    public Rigidbody2D rb;
    
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!targetMask.Contains(other.gameObject.layer)) return;

        InactiveCollision();
        transform.rotation = Quaternion.Euler(0,0,transform.position.GetAngle(other.contacts[0].point)-90);
        transform.position = other.contacts[0].point + other.contacts[0].normal * .01f;
        transform.parent = other.transform;
        topDownPhysics2D.ResetPhysics();
        HideHead();
    }

    public void ActiveCollision()
    {
        col.isTrigger = false;
        rb.isKinematic = false;
    }

    public void InactiveCollision()
    {
        col.isTrigger = true;
        rb.isKinematic = true;
    }

    public void ShowHead() => head.gameObject.SetActive(true);
    public void HideHead() => head.gameObject.SetActive(false);

    public void OnTriggerEnter2D(Collider2D other)
    {
        ArrowPicker arrowPicker = other.gameObject.GetComponent<ArrowPicker>();
        if (!arrowPicker) return;
        arrowPicker.Pick(this);
    }
}
