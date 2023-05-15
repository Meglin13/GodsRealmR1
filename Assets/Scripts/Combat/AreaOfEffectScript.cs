using MyBox;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowRadiusCircleScript))]
public class AreaOfEffectScript : SpawningObject
{
    [SerializeField]
    private AreaType areaType;

    [SerializeField]
    [ReadOnly]
    private float LifeTime;

    [SerializeField]
    [ReadOnly]
    private float TimeBetweenAttacks;

    [SerializeField]
    [ReadOnly]
    private float passedTime;

    [SerializeField]
    [ReadOnly]
    private float passedTimeBetweenAttacks;

    ShowRadiusCircleScript area;

    public override void Spawn(IDamageable dude, Skill skill)
    {
        base.Spawn(dude, skill);
        LifeTime = skill.DurationInSecs;
        TimeBetweenAttacks = skill.PeriodicDamageTime;
        area.Draw(Radius);

        passedTime = 0;
        passedTimeBetweenAttacks = 0;
    }

    private void Awake()
    {
        area = GetComponent<ShowRadiusCircleScript>();
    }

    private void Update()
    {
        if (passedTime < LifeTime)
        {
            passedTime += Time.deltaTime;
            passedTimeBetweenAttacks += Time.deltaTime;

            if (passedTimeBetweenAttacks > TimeBetweenAttacks)
            {
                CheckHits();
                passedTimeBetweenAttacks = 0;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void CheckHits()
    {
        int layer = areaType == AreaType.Attack ? 1 << DealerStats.EnemyLayer : 1 << DealerStats.EntityLayer;

        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, Radius, layer);

        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
                MiscUtilities.GetInterfaces(out List<IDamageable> interfaceList, item.gameObject);
                foreach (IDamageable damageable in interfaceList)
                {
                    if (areaType == AreaType.Attack)
                    {
                        damageable.TakeDamage(DealerStats, skill.DamageMultiplier.GetFinalValue(), false);

                        if (skill.StunTime > 0)
                            damageable.Stun(skill.StunTime);

                        if (skill.EnemyModifier != null)
                            GameManager.Instance.StartCoroutine(damageable.AddModifier(skill.EnemyModifier));
                    }
                    else
                    {
                        damageable.GiveSupport(skill.DamageMultiplier.GetFinalValue() * DealerStats.ModifiableStats[StatType.Health].GetFinalValue(), StatType.Health);
                    }
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, Radius);
    }
}