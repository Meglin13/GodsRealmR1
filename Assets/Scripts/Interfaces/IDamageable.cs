using UnityEngine;

public interface IDamageable
{
    public GameObject gameObject { get; }
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public EntityStats EntityStats { get; set; }
    void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanParry);
    void HitVFX(Vector3 hitPoint);
    void Stun(float StunTime);
    void Death();
}