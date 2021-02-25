using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using static ContactRaycaster2D;

public class ContactRaycaster2D : MonoBehaviour
{
    public float skin = .05f;
    public float minRayLength = .05f;
    public Action<Collider2D> OnRaycastHit;

    public Action<Contact> OnContactEnter;
    public Action<Contact> OnContactExit;
    public Action<Contact> OnContactStay;

    public Collider2D col;
    public bool shortCircuitEvaluation = true;

    public LayerMask contactMask;

    [OnValueChanged("RecalculateDensity")]
    public Vector2Int rays = new Vector2Int(1, 1);

    [OnValueChanged("RecalculateRaysCount")]
    public Vector2 density = new Vector2(1, 1);

    public List<Vector2> points = new List<Vector2>();

    private readonly Vector2[] corners = new Vector2[4];

    public Vector2 raySeparation = new Vector2();

    [Flags]
    public enum Contact {None = 0, Left = 1 << 0, Right = 1 << 1, Up = 1 << 2, Down = 1 << 3, Horizontal = Left | Right, Vertical = Up | Down}
    [SerializeField] private Contact _contacts;

    public Contact Contacts => _contacts;

    private void Start()
    {
        RecalculateDensity();
        RecalculateBounds();
    }

    private void OnValidate()
    {
        if (rays.x < 2) rays.x = 2;
        if (rays.y < 2) rays.y = 2;
        RecalculateDensity();
        if(!col)
            col = GetComponent<Collider2D>();
    }

    private void RecalculateDensity()
    {
        density = new Vector2(1f / rays.x, 1f / rays.y);
    }

    private void RecalculateRaysCount()
    {
        rays = new Vector2Int(Mathf.RoundToInt(1f * density.x), Mathf.RoundToInt(1f * density.y));
    }

    [Button]
    private void RecalculateBounds()
    {
        corners[0] = transform.position + new Vector3(-col.bounds.extents.x, col.bounds.extents.y);
        corners[1] = transform.position + new Vector3(col.bounds.extents.x, col.bounds.extents.y);
        corners[2] = transform.position + new Vector3(col.bounds.extents.x, -col.bounds.extents.y);
        corners[3] = transform.position + new Vector3(-col.bounds.extents.x, -col.bounds.extents.y);

        raySeparation.x = Vector2.Distance(corners[0], corners[1]) / (rays.x - 1);
        raySeparation.y = Vector2.Distance(corners[0], corners[3]) / (rays.y - 1);
    }

    public Contact ProcessWithVelocity(ref Vector2 velocity)
    {
        RecalculateBounds();

        var result = ShotRay(corners[1], new Vector2(0, -raySeparation.y), ref _contacts, rays.y, Vector2.right, velocity.x, Contact.Right);
        if (velocity.x > 0 && result >= 0) velocity.x = result;

         result = ShotRay(corners[3], new Vector2(0, raySeparation.y), ref _contacts, rays.y, Vector2.left, -velocity.x, Contact.Left);
        if (velocity.x < 0 && result >= 0) velocity.x = -result;
      

        result = ShotRay(corners[0], new Vector2(raySeparation.x, 0), ref _contacts, rays.x, Vector2.up, velocity.y, Contact.Up);
        if (velocity.y > 0 && result >= 0) velocity.y = result;

        result = ShotRay(corners[2], new Vector2(-raySeparation.x, 0), ref _contacts, rays.x, Vector2.down, -velocity.y, Contact.Down);
        if (velocity.y < 0 && result >= 0) velocity.y = -result;

        return _contacts;
    }

    private float ShotRay(Vector2 startPosition, Vector2 displacement, ref Contact contacts, int count, Vector2 rayDirection, float rayLength, Contact contactDir)
    {
        bool anyHit = false;

        float distance = Mathf.Max(rayLength,0);

        if(rayLength > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 position = startPosition + rayDirection * skin + displacement*i;
                var hit = Physics2D.Raycast(position, rayDirection, rayLength - skin, contactMask);
                if (hit)
                {
                    anyHit = true;
                    OnRaycastHit?.Invoke(hit.collider);

                    distance = Mathf.Min(distance, Mathf.Abs(Vector2.Distance(position, hit.point)));
                    Debug.DrawLine(position, hit.point, Color.green);

                    if (shortCircuitEvaluation)
                        break;
                }
                else
                    Debug.DrawRay(position, rayDirection * rayLength, Color.red);
            }

            if (anyHit)
            {
                if (contacts.Contains(contactDir))
                    OnContactStay?.Invoke(contactDir);
                else
                    OnContactEnter?.Invoke(contactDir);

                contacts |= contactDir;
            }
            else if (contacts.Contains(contactDir))
            {
                contacts &= ~contactDir;
                OnContactExit?.Invoke(contactDir);
            }
        }

        if (rayLength <= 0)
            ShotRay(startPosition, displacement, ref contacts, count, rayDirection, minRayLength, contactDir);

        return distance;
    }


}
public static class ContactExtension
{
    public static bool Contains(this Contact self, Contact flag) => (self & flag) == flag;
}