using ObjectPooling;
using UnityEngine;

namespace Characters
{
    public class Marfa : CharacterScript, ICharacter
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
            var totem = MiscUtilities.SpawnObjectInFrontOfObject(this, Distract.SpawningObject, 5);
            totem.Spawn(this, Distract);
        }

        public void SpecialAbility()
        {
            GameManager.Instance.partyManager.GiveSupportToAll(EntityStats.ModifiableStats[StatType.Health].GetProcent() * Special.DamageMultiplier.GetFinalValue(), StatType.Health);
        }

        public void UltimateAbility()
        {

        }
    } 
}