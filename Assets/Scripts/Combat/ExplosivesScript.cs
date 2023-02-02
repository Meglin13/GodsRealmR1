using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class ExplosivesScript : MonoBehaviour, IWeapon
{
    public GameObject BoomVFX;
    public AudioSource audioSource;

    public EntityStats DealerStats { get; set; }
    public float Mult { get; set; }
    public float Length { get; set; }
    public List<Collider> hasHitted { get; set; }

    private float StunTime;

    [HideInInspector]
    public int TargetLayer;

    private void Awake()
    {
        hasHitted = new List<Collider>();
    }

    public void Init(EntityStats entityStats, float Mult, float Radius, float StunTime)
    {
        DealerStats = entityStats;
        TargetLayer = 1 << TargetLayer;
        this.Length = Radius;
        this.Mult = Mult;
        this.StunTime = StunTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (BoomVFX)
        {
            GameObject vfx = Instantiate(BoomVFX, transform.position, transform.rotation);
            Destroy(vfx, 1f);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, this.Length, TargetLayer);
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
                        damageable.TakeDamage(DealerStats, Mult, false);
                        damageable.Stun(StunTime);
                    }
                }
            }
        }
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, this.Length);
    }
}
