using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class BulletsScript : MonoBehaviour, IWeapon
{
    //UNDONE: Скрипт снарядов
    public EntityStats DealerStats { get; set; }
    public float Mult { get; set; }
    public float Length { get; set; }
    public List<Collider> hasHitted { get; set; }

    //Параметры снаряда
    public bool IsAutoAim;
    public float LifeTime = 5f;
    public float HitRadius = 5f;
    public float Speed;

    //Цель для автоаима
    [HideInInspector]
    public int TargetLayer;
    [HideInInspector]
    public GameObject target;

    public void Init(EntityStats entityStats, float Mult, float Length, float StunTime)
    {
        DealerStats = entityStats;
        TargetLayer = 1 << TargetLayer;
        this.Length = Length;
        this.Mult = Mult;
    }

    void Start()
    {
        hasHitted = new List<Collider>();

        Destroy(gameObject, LifeTime);

        //TODO: Автонаведение
        //if (IsAutoAim)
        //{
        //    AIUtilities.FindNearestEntity(transform, );
        //}
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, HitRadius, TargetLayer);
        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
                if (!hasHitted.Contains(item))
                {
                    hasHitted.Add(item);

                    MiscUtilities.GetInterfaces(out List<IDamageable> interfaceList, item.gameObject);
                    foreach (IDamageable damageable in interfaceList)
                        damageable.TakeDamage(DealerStats, Mult, false);
                }
            }
            Destroy(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, HitRadius);
    }
}