using UnityEngine;

public abstract class SpawningObject : MonoBehaviour
{
    internal IDamageable dealer;
    internal EntityStats DealerStats;
    internal Skill skill;

    internal float Radius;

    public virtual void Spawn(IDamageable dealer, Skill skill)
    {
        this.dealer = dealer;
        this.skill = skill;
        Radius = skill.Radius;
        DealerStats = dealer.EntityStats;
    }

    public virtual void OnDisable()
    {
        dealer = null;
        skill = null;
        Radius = 0;
        DealerStats = null;
    }
}