using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Contact = ContactRaycaster2D.Contact;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPhysics2D : MonoBehaviour
{
    public Action<Vector2> OnForceAdded;

    [SerializeField] private Vector2 velocity;
    public Vector2 Velocity => velocity;
    [SerializeField] private ContactRaycaster2D contactRaycaster2D;
    [SerializeField] private Rigidbody2D rigidbody2D;
    
    Vector2 displacement;

    [SerializeField] public float deacceleration = .4f;
    public Vector2 currentGravity => Physics2D.gravity;

    [Button]
    public void ChangeGravity(Vector2 nValue) => Physics2D.gravity = nValue;

    private void OnValidate()
    {
        if (!contactRaycaster2D) contactRaycaster2D = GetComponent<ContactRaycaster2D>();
        if (!rigidbody2D) rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        displacement = velocity * Time.fixedDeltaTime;

        if(contactRaycaster2D) contactRaycaster2D.ProcessWithVelocity(ref displacement);

        velocity = displacement / Time.fixedDeltaTime;

        rigidbody2D.MovePosition((Vector2)transform.position + displacement);

        velocity.x *= 1 - Time.fixedDeltaTime * deacceleration;
        velocity.y *= 1 - Time.fixedDeltaTime * deacceleration;
    }

    [Button]
    public void AddForce(Vector2 force)
    {
        velocity += force;
        OnForceAdded?.Invoke(force);
    }

    [Button]
    public void SetVelocity(Vector2 nVelocty)
    {
        velocity = nVelocty;
    }

    public void ResetPhysics()
    {
        rigidbody2D.velocity = velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0;
    }
}