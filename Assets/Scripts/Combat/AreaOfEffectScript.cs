using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(ShowRadiusCircleScript))]
public class AreaOfEffectScript : MonoBehaviour
{
    [SerializeField]
    private float LifeTime;
    [SerializeField]
    private float TimeBetweenAttacks;
    private float Radius;

    private EntityStats DealerStats;
    private Skill skill;

    [SerializeField]
    private float passedTime;
    [SerializeField]
    private float passedTimeBetweenAttacks;

    public void Init(EntityStats entityStats, Skill skill)
    {
        this.skill = skill;

        LifeTime = skill.DurationInSecs;
        Radius = skill.Radius;
        TimeBetweenAttacks = skill.PeriodicDamageTime;

        DealerStats = entityStats;

        GetComponent<ShowRadiusCircleScript>().radius = Radius;
        GetComponent<ShowRadiusCircleScript>().Draw();
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
            Destroy(gameObject);
        }
    }

    public void CheckHits()
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, Radius, 1 << DealerStats.EnemyLayer);

        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
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

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, Radius);
    }
}