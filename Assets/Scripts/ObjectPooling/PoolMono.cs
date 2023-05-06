using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ObjectPooling
{
    [Serializable]
    public class PoolMono<T> where T : MonoBehaviour
    {
        private T Prefab { get; }
        private ObjectAmountList<T> Prefabs { get; }
        public bool AutoExpand { get; set; }
        public Transform Container { get; }

        private List<T> pool;

        public PoolMono(ObjectAmountList<T> prefabs, int count, Transform container)
        {
            this.Prefabs = prefabs;
            this.Container = container;

            this.CreatePool(count);

        }

        private void CreatePool(int count)
        {
            this.pool = new List<T>();

            foreach (var item in Prefabs.GetList())
            {
                CreateObject(item);
            }
        }

        private T CreateObject(T prefab, bool isActiveByDiffault = false)
        {
            var createdObject = UnityEngine.Object.Instantiate(prefab, Container);
            
            createdObject.gameObject.SetActive(isActiveByDiffault);
            createdObject.gameObject.transform.SetParent(Container);

            pool.Add(createdObject);

            return createdObject;
        }

        public bool HasFreeElement(T prefab, out T element)
        {
            foreach (var item in pool)
            {
                if (!item.gameObject.activeInHierarchy & prefab == PrefabUtility.GetCorrespondingObjectFromSource(item))
                {
                    element = item;
                    item.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        public T GetFreeElement(T prefab)
        {
            if (HasFreeElement(prefab, out var element))
                return element;

            if (AutoExpand)
                return CreateObject(prefab, true);

            throw new System.Exception($"No elements of type {typeof(T)}");
        }
    }
}
