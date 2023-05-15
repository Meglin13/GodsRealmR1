using ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
        }

        public void SpecialAbility()
        {
            
        }

        public void UltimateAbility()
        {
            PartyManager.Instance.ApplyBuffOnTeam(new Modifier(StatType.Attack, Special.DamageMultiplier.GetFinalValue(), ModifierAmountType.Value, Special.DurationInSecs, ModType.Buff));
        }
    }
}