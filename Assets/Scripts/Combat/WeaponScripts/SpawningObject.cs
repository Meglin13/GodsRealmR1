using UnityEngine;

public abstract class SpawningObject : MonoBehaviour
{
    internal IDamageable dude;
    internal EntityStats DealerStats;
    internal Skill skill;

    internal float Radius;

    public virtual void Spawn(IDamageable dude, Skill skill)
    {
        this.dude = dude;
        this.skill = skill;
        Radius = skill.Radius;
        DealerStats = dude.EntityStats;
    }

    public virtual void OnDisable()
    {
        dude = null;
        skill = null;
        Radius = 0;
        DealerStats = null;
    }
}