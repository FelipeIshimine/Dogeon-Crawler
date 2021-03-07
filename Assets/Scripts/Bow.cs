using System.Collections;
using System.Collections.Generic;
using Pooling.Poolers;
using Sirenix.OdinInspector;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public float distanceFromCenter = 1;
    public Transform firePoint;
    public SetPooler pool;

    [Button]
    public void Aim(Vector2 dir)
    {
        transform.rotation = Quaternion.Euler(0, 0, dir.AsAngle() - 90);
        transform.localPosition =  distanceFromCenter * transform.up;
    }

    [Button]
    public void Aim(float zAngle)
    {
        transform.rotation = Quaternion.Euler(0, 0, zAngle- 90);
        transform.localPosition =  distanceFromCenter * transform.up;
    }

    [Button]
    public void Fire()
    {
       var go =  pool.Dequeue();
       Projectile projectile = go.GetComponent<Projectile>();
       projectile.SelfPoolable.pool = pool;
       var projectileTransform = projectile.transform;
       var myTransform = transform;
       projectileTransform.position = firePoint.position;
       projectileTransform.rotation = firePoint.rotation;
       projectile.Fire();
    }
}
