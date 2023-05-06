using UnityEngine;

namespace ObjectPooling
{
    public class PoolerScript<T> : MonoBehaviour, ISingle where T : MonoBehaviour
    {
        public static PoolerScript<T> Instance;
        public void Initialize() => Instance = this;

        [SerializeField]
        private int poolCount = 10;
        [SerializeField]
        private bool autoExpand = false;

        internal PoolMono<T> pool;

        public ObjectAmountList<T> objectsList;

        private void Awake()
        {
            Initialize();

            pool = new PoolMono<T>(objectsList, poolCount, gameObject.transform)
            {
                AutoExpand = autoExpand
            };
        }

        public virtual T CreateObject(T prefab, Vector3 spawnPoint)
        {
            var obj = pool.GetFreeElement(prefab);
            obj.gameObject.transform.position = spawnPoint;

            return obj;
        }

        public virtual T CreateObject(T prefab, Vector3 gunpoint, IDamageable dealer, Skill skill)
        {
            return null;
        }
    }
}
