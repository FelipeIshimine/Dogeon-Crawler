using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Contact = ContactRaycaster2D.Contact;

[RequireComponent(typeof(ContactRaycaster2D))]
public class PlatformerPhysics2D : MonoBehaviour
{
    public Action<Vector2> OnForceAdded;

    [SerializeField] private Vector2 velocity;
    public Vector2 Velocity => velocity;
    [SerializeField] private ContactRaycaster2D contactRaycaster2D;

    public float gravityMultiplier = 1;
    Vector2 displacement;

    [SerializeField] public float horizontalDeacceleration = .4f;
    public Vector2 currentGravity => Physics2D.gravity;

    [Button]
    public void ChangeGravity(Vector2 nValue) => Physics2D.gravity = nValue;

    private void OnValidate()
    {
        if (!contactRaycaster2D) contactRaycaster2D = GetComponent<ContactRaycaster2D>();
    }

    private void FixedUpdate()
    {
        velocity += Time.fixedDeltaTime * gravityMultiplier * Physics2D.gravity;
        displacement = velocity * Time.fixedDeltaTime;

        Contact contacts = contactRaycaster2D.ProcessWithVelocity(ref displacement);

        velocity = displacement / Time.fixedDeltaTime;

        transform.Translate(displacement, Space.World);

        velocity.x *= 1 - Time.fixedDeltaTime * horizontalDeacceleration;
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
}