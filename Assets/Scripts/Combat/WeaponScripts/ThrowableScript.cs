using ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ThrowableScript : SpawningObject
{
    private Rigidbody rb;

    [HideInInspector]
    public List<Collider> hasHitted = new List<Collider>();

    [SerializeField]
    private float Speed;
    [SerializeField]
    private ForceMode mode;

    public override void Spawn(IDamageable dude, Skill skill)
    {
        base.Spawn(dude, skill);

        hasHitted = new List<Collider>();
        rb.AddForce(dude.gameObject.transform.forward * Speed, mode);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != dude.gameObject)
        {
            CheckHits();
            OnHit(collision.GetContact(0).point);
            gameObject.SetActive(false);
        }
    }

    public void CheckHits()
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, skill.Radius, 1 << DealerStats.EnemyLayer);

        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
                if (!hasHitted.Contains(item))
                {
                    hasHitted.Add(item);

                    MiscUtilities.GetInterfaces(out List<IDamageable> interfaceList, item.gameObject);

                    foreach (IDamageable damageable in interfaceList)
                    {
                        damageable.TakeDamage(DealerStats, skill.DamageMultiplier.GetFinalValue(), false);

                        if (skill.StunTime > 0)
                            damageable.Stun(skill.StunTime);

                        if (skill.EnemyModifier != null)
                            GameManager.Instance.StartCoroutine(damageable.AddModifier(skill.EnemyModifier));
                    }
                }
            }
        }
    }

    public virtual void OnHit(Vector3 hitPoint) => VFXPool.Instance.CreateObject(GameManager.Instance.HitVFX, hitPoint);

    private void OnDrawGizmos()
    {
        if (skill != null)
            Gizmos.DrawWireSphere(transform.position, skill.Radius);
    }
}