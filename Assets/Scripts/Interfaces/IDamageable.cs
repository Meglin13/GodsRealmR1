using System.Collections;
using UnityEngine;

public interface IDamageable
{
    GameObject gameObject { get; }
    EntityStats EntityStats { get; set; }
    float CurrentHealth { get; set; }
    int EnemyLayer { get; set; }
    string EnemyTag { get; set; }
    void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanBlock);
    IEnumerator AddModifier(Modifier modifier);
    void HitVFX(Vector3 hitPoint);
    void Stun(float StunTime);
    void Death();
}