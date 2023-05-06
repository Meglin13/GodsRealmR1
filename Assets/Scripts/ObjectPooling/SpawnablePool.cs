using UnityEngine;

namespace ObjectPooling
{
    public class SpawnablePool : PoolerScript<SpawningObject>
    {
        public override SpawningObject CreateObject(SpawningObject bull, Vector3 gunpoint, IDamageable dealer, Skill skill)
        {
            var bullet = pool.GetFreeElement(bull);
            bullet.gameObject.transform.position = gunpoint;
            bullet.Spawn(dealer, skill);

            return bullet;
        }
    }
}