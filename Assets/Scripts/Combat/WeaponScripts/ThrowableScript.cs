using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ThrowableScript : MonoBehaviour
{
    private Rigidbody rb;
    public IDamageable dude;

    [HideInInspector]
    public EntityStats DealerStats;
    [HideInInspector]
    public List<Collider> hasHitted = new List<Collider>();

    [SerializeField]
    private float Speed;
    [SerializeField]
    private ForceMode mode;
    [HideInInspector]
    public Skill skill;

    public virtual void Init(IDamageable dude, Skill skill)
    {
        this.dude = dude;
        this.skill = skill;
        this.DealerStats = dude.EntityStats;

        rb = GetComponent<Rigidbody>();
        hasHitted = new List<Collider>();

        Throw();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != dude.gameObject)
        {
            CheckHits();
            OnHit(collision.GetContact(0).point);
            Destroy(gameObject, 0.1f);
        }
    }

    public virtual void Throw()
    {
        rb.AddForce(dude.gameObject.transform.forward * Speed, mode);
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
                            damageable.AddModifier(skill.EnemyModifier);
                    }
                }
            }
        }
    }

    public virtual void OnHit(Vector3 hitPoint)
    {
        GameObject vfx = Instantiate(GameManager.Instance.HitVFX, hitPoint, Quaternion.Euler(0, 0, 0));
        Destroy(vfx, 0.6f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, skill.Radius);
    }
}