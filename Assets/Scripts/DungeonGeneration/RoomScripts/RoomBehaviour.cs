using MyBox;
using UnityEditor;
using UnityEngine;

namespace DungeonGeneration
{
    public enum RoomBehaviourType
    { Battle, Shop, Buffer, FreeChest, Heal, Boss, Misc }

    public abstract class RoomBehaviour : ScriptableObject
    {
        [SerializeField]
        private protected RoomBehaviourType behaviourType;
        public RoomBehaviourType BehaviourType 
        { 
            get => behaviourType;
            protected set => behaviourType = value; 
        }

        [SerializeField]
        private protected GameObject InstantiatingProp;

        private RoomScript room;
        public RoomScript Room { get => room; }

        [SerializeField]
        private float spawnRate;
        public float SpawnRate { get => spawnRate; }

        [SerializeField]
        private bool canHaveSameNeighbors = true;
        public bool CanHaveSameNeighbors { get => canHaveSameNeighbors; }

        [ConditionalField("canHaveSameNeighbors", true)]
        [SerializeField]
        private int sameNeighborsRadius;
        public int SameNeighborsRadius { get => sameNeighborsRadius; }

        [SerializeField]
        public virtual void Initialize(RoomScript room) => this.room = room;

        public void SetProp(GameObject Prop, bool Active = true)
        {
            if (Prop)
            {
                InstantiatingProp = Instantiate(Prop);
                InstantiatingProp.transform.position = Room.transform.position;
                InstantiatingProp.SetActive(Active);

                InstantiatingProp.transform.SetParent(Room.PropsContainer.transform);
            }
        }

        public abstract void OnRoomEnter();
        public abstract void OnRoomExit();
    }
}