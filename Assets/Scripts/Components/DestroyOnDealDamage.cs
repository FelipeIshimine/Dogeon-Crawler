using UnityEngine;

public class DestroyOnDealDamage : MonoBehaviour
{
    public DamageOnContact damageOnContact;

    void OnValidate()
    {
        if (!damageOnContact) damageOnContact = gameObject.GetComponent<DamageOnContact>();
    }

    private void Awake()
    {
        damageOnContact.OnDamageDone += OnDamageDone;
    }

    private void OnDestroy()
    {
        damageOnContact.OnDamageDone -= OnDamageDone;
    }

    private void OnDamageDone(int damage, Health arg2)
    {
        Destroy(gameObject);
    }
}