using ObjectPooling;
using UnityEngine;

namespace Characters
{
    public class Ardalion : CharacterScript, ICharacter
    {
        private bool IsUltimateActive = false;

        public override void Initialize()
        {
            Character = this;
            base.Initialize();
            IsUltimateActive = false;
        }

        public void Attack()
        {
            ////if (IsUltimateActive)
            ////{
            //    Transform transform = gameObject.transform;
            //    Vector3 gunpoint = transform.position + transform.forward + transform.up;

            //    var slash = SpawnablePool.Instance.CreateObject(Ultimate.SpawningObject, gunpoint);
            //    slash.Spawn(this, Ultimate);

            ////}
        }

        public void DistractionAbility()
        {
            Transform transform = gameObject.transform;
            Vector3 gunpoint = transform.position + transform.forward + transform.up;

            Debug.Log($"{this} {Distract}");
            var distract = SpawnablePool.Instance.CreateObject(Distract.SpawningObject, gunpoint);
            distract.Spawn(this, Distract);
        }

        private int stacks = 0;
        private Modifier mod = new Modifier(StatType.Attack, 20, ModifierAmountType.Procent, 10, ModType.Buff);

        public void SpecialAbility()
        {
            stacks++;
            if (stacks == 3)
                GameManager.Instance.StartCoroutine(AddModifier(mod));

            MeleeWeapon.Init(EntityStats, Special.DamageMultiplier.GetFinalValue(), false);
        }

        public void UltimateAbility()
        {
            StartCoroutine(AddModifier(new Modifier(StatType.CritChance, 20, ModifierAmountType.Value, Ultimate.DurationInSecs, ModType.Buff)));

            IsUltimateActive = true;
            GameManager.Instance.StartCoroutine(MiscUtilities.Instance.ActionWithDelay(Ultimate.DurationInSecs, () => IsUltimateActive = false));
        }
    } 
}