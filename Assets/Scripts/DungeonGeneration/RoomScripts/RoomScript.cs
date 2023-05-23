using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration
{
    public enum WorldSide { North = 1, East, South, West }

    /// <summary>
    /// Класс хранящий информацию о проходах и опредлеяющий их логику
    /// </summary>
    [Serializable]
    public class DoorwayObject
    {
        [ReadOnly]
        public WorldSide Side;

        public GameObject Wall;
        public GameObject Doorway;
        public GameObject Door;

        public bool IsDoorActive;

        /// <summary>
        /// Установка стены или прохода
        /// </summary>
        public void SetDoorway()
        {
            Wall.active = !IsDoorActive;
            Doorway.active = IsDoorActive;
        }

        /// <summary>
        /// Открытие/закрытие двери
        /// </summary>
        /// <param name="IsOpened"></param>
        public void SetDoor(bool IsOpened)
        {
            if (Door != null)
                Door.active = Doorway.active && !IsOpened;
        }
    }

    [SelectionBase]
    public class RoomScript : MonoBehaviour
    {
        public Vector2Int Size;

        [SerializeField]
        private bool canHaveBehaviour = true;
        public bool CanHaveBehaviour { get => canHaveBehaviour; }
        [SerializeField]
        private RoomBehaviour Behaviour;
        private bool isRoomCleared = false;
        public bool IsRoomCleared { get => isRoomCleared; }

        public event Action OnRoomClear = delegate { };
        public event Action OnRoomEnter = delegate { };
        public event Action OnRoomExit = delegate { };

        public Vector2Int coordinates;

        public GameObject[] Props;
        public GameObject PropsContainer;
        [SerializeField] private GameObject DecorationContainer;

        [HideInInspector]
        public int OpenedDoorsCount;
        public List<DoorwayObject> DoorwaysList = new List<DoorwayObject>(4)
        {
            new DoorwayObject() { Side = WorldSide.North},
            new DoorwayObject() { Side = WorldSide.East},
            new DoorwayObject() { Side = WorldSide.South},
            new DoorwayObject() { Side = WorldSide.West}
        };

        private void OnDisable()
        {
            OnRoomClear = null;
            OnRoomEnter = null;
            OnRoomExit = null;
        }

        public void OnRoomEnterTrigger() => OnRoomEnter();
        public void OnRoomExitTrigger() => OnRoomExit();

        private void Awake()
        {
            gameObject.isStatic = true;

            SetDoorways();

            OnRoomClear += () => SetDoorsState(true);

            if (Behaviour)
                SetBehaviour(Behaviour);
        }

        public void RoomClear()
        {
            isRoomCleared = true;
            OnRoomClear();
        }

        public void SetBehaviour(RoomBehaviour behaviour)
        {
            if (CanHaveBehaviour)
            {
                //foreach (Transform child in PropsContainer.transform)
                //{
                //    DestroyImmediate(child.gameObject);
                //}

                this.Behaviour = Instantiate(behaviour);
                Behaviour.Initialize(this);

                OnRoomEnter += Behaviour.OnRoomEnter;
                OnRoomExit += Behaviour.OnRoomExit; 
            }
        }

        [ButtonMethod]
        public void SetDecoration()
        {
            foreach (Transform child in DecorationContainer.transform)
            {
                bool active = UnityEngine.Random.Range(0, 11) % 2 == 0;
                child.gameObject.SetActive(active);
            }
        }

#if UNITY_EDITOR

        [ButtonMethod]
        public void HideDecor()
        {
            foreach (Transform child in DecorationContainer.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

#endif

        #region [Doors]
        /// <summary>
        /// Установка состояния дверей
        /// </summary>
        /// <param _name="sides">Массив информации о ближайших комнатах. 
        /// Двери открываются по часовой стрелке, начиная с севера: СЕВЕР, ВОСТОК, ЮГ, ЗАПАД</param>
        public void SetDoorways(bool[] sides)
        {
            OpenedDoorsCount = 0;

            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i])
                {
                    DoorwaysList[i].IsDoorActive = true;
                    DoorwaysList[i].SetDoorway();
                    OpenedDoorsCount++;
                }
            }
        }

        /// <summary>
        /// Установка проходов
        /// </summary>
        [ButtonMethod]
        public void SetDoorways()
        {
            foreach (var door in DoorwaysList)
            {
                door.SetDoorway();
            }
        }

        /// <summary>
        /// Метод используется для того, чтобы закрывать именно двери, а не проходы
        /// </summary>
        /// <param _name="IsOpened">Открыть двери или нет</param>
        public void SetDoorsState(bool IsOpened)
        {
            foreach (var door in DoorwaysList)
            {
                door.SetDoor(IsOpened);
            }
        }

#if UNITY_EDITOR

        [ButtonMethod]
        public void CloseDoors()
        {
            SetDoorsState(false);
        }

        [ButtonMethod]
        public void OpenDoors()
        {
            SetDoorsState(true);
        }

#endif
        #endregion
    } 
}