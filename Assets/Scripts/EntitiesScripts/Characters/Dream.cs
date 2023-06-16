using ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    internal class Dream : CharacterScript, ICharacter
    {
        public override void Initialize()
        {
            Character = this;
            base.Initialize();
        }

        public void Attack()
        {
            Skill attackSkill = EntityStats.NormalAttackSkill;

            Transform transform = gameObject.transform;
            Vector3 gunpoint = transform.position + transform.forward + transform.up;

            var attack = SpawnablePool.Instance.CreateObject(attackSkill.SpawningObject, gunpoint);
            attack.Spawn(this, attackSkill);
        }

        public void DistractionAbility()
        {
            var skill = Distract;

            //TODO: Сделать метод получения айдемеджебелов
            Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, skill.Radius, EntityStats.EnemyLayer);

            if (hits.Length > 0)
            {
                foreach (var item in hits)
                {
                    MiscUtilities.GetInterfaces(out List<IDamageable> interfaceList, item.gameObject);
                    foreach (IDamageable damageable in interfaceList)
                    {
                            damageable.TakeDamage(EntityStats, skill.DamageMultiplier.GetFinalValue(), false);

                                damageable.Stun(skill.StunTime);
                    }
                }
            }
        }

        public void SpecialAbility()
        {
            PartyManager.Instance.ApplyBuffOnTeam(new Modifier(StatType.Speed, 10, ModifierAmountType.Procent, Special.DurationInSecs, ModType.Buff));
        }

        public void UltimateAbility()
        {
            PartyManager.Instance.ApplyBuffOnTeam(new Modifier(StatType.Attack, EntityStats.ModifiableStats[StatType.Attack].GetProcent() * Ultimate.DamageMultiplier.GetFinalValue(), ModifierAmountType.Value, Special.DurationInSecs, ModType.Buff));
        }
    }
}