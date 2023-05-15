using MyBox;
using UnityEngine;

namespace DungeonGeneration
{
    public enum RoomBehaviourType
    { Battle, Shop, Buffer, FreeChest, Corridor, Heal, BossPortal, Misc }

    public abstract class RoomBehaviour : ScriptableObject
    {
        [ReadOnly]
        public RoomBehaviourType BehaviourType;

        public GameObject InstantiatingProp;
        internal RoomScript Room;
        public float SpawnRate;

        [SerializeField]
        public virtual void Initialize(RoomScript room)
        {
            this.Room = room;
        }

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