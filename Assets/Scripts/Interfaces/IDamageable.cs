using System.Collections;
using UnityEngine;

public interface IDamageable
{
    GameObject gameObject { get; }
    EntityStats EntityStats { get; }
    float CurrentHealth { get; set; }
    int EnemyLayer { get; }
    string EnemyTag { get; }
    void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanBlock);
    IEnumerator AddModifier(Modifier modifier);
    void GiveSupport(float Amount, StatType type);
    void HitVFX(Vector3 hitPoint);
    void Stun(float StunTime);
    void Death();
}