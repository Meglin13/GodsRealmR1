using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [HideInInspector]
    public bool canDealDamage;
    private List<GameObject> hasHitted;
    private int layerMask;
    private EntityStats DealerStats;
    private float Mult;
    public Stat WeaponLength;
    private bool CanParry;

    [SerializeField]
    private float BaseWeaponLength = 1.4f;
    private float CurrentWeaponLength;

    public void Init(EntityStats entityStats, float Mult, bool CanParry)
    {
        DealerStats = entityStats;
        this.CanParry = CanParry;
        this.Mult = Mult;
        CurrentWeaponLength = WeaponLength.GetFinalValue();
    }

    private void Awake()
    {
        WeaponLength = new Stat(BaseWeaponLength);
    }

    void Start()
    {
        canDealDamage = false;
        hasHitted = new List<GameObject>();
        layerMask = GetComponentInParent<CharacterScript>() != null ? 1 << 8 : 1 << 6;
        CurrentWeaponLength += 1f;
    }

    void FixedUpdate()
    {
        if (canDealDamage)
        {
            if (Physics.Raycast(transform.position - transform.up, transform.up, out RaycastHit hit, CurrentWeaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out IDamageable enemy) && !hasHitted.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(DealerStats, Mult, CanParry);

                    enemy.HitVFX(hit.point);

                    hasHitted.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasHitted.Clear();
    }
    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - transform.up, transform.position + (transform.up * BaseWeaponLength));
    }
}