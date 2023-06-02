using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace DungeonGeneration
{
    [Serializable]
    public class GenerationParameters
    {
        public Vector2Int GridSize;

        public int numberOfRooms = 5;
        public int numberOfEvents;

        public int Seed = 0;
    }

    public class DungeonGeneratorScript : MonoBehaviour, ISingle
    {
        public static DungeonGeneratorScript Instance;
        public void Initialize()
        {
            Instance = this;
        }

        [Foldout("Generation", true)]
        public Random random;
        [SerializeField]
        private GenerationParameters GenerationParameters = new GenerationParameters();
        public int SeedOutput;

        private RoomScript[,] Rooms;
        private int TESTMODX = 1, TESTMODY = -1;

        [SerializeField]
        private List<RoomBehaviour> roomBehaviours;
        [SerializeField]
        private SnuffleBag<RoomBehaviour> behavioursBag;

        [Foldout("Prefabs", true)]
        [SerializeField]
        private List<RoomScript> RoomsPrefabs = new List<RoomScript>();
        [SerializeField]
        private List<RoomScript> ConnectionRoomsPrefabs = new List<RoomScript>();
        [SerializeField]
        private GameObject BossRoom;
        [SerializeField]
        private List<RoomScript> PlacedRooms = new List<RoomScript>();
        [SerializeField]
        private GameObject Dungeon;

#if UNITY_EDITOR

        [Foldout("Statistic", true)]
        public int FreeChestAmount;
        public int BuffsAmount;
        public int EnemiesAmount;
        public int FontainesAmount;
        public int BehaviourAmount;
#endif

        public event Action OnGenerationComplited = delegate { };

        private void Start()
        {
            Initialize();

            if (RunManager.Params.GenerationParameters != null)
                GenerationParameters = RunManager.Params.GenerationParameters;

            GenerateDungeon();

            foreach (var item in GameManager.Instance.partyManager.PartyMembers)
                item.agent.Warp(PlacedRooms[0].transform.position);
        }

        private void OnDisable() => OnGenerationComplited = null;

        [ButtonMethod]
        public void GenerateDungeon()
        {
            var seed = GenerationParameters.Seed == 0
                ? UnityEngine.Random.Range(int.MinValue, int.MaxValue)
                : GenerationParameters.Seed;

            SeedOutput = seed;

            random = new Random(seed);

            Rooms = new RoomScript[GenerationParameters.GridSize.x, GenerationParameters.GridSize.y];

            if (PlacedRooms.Count > 1)
                ClearDungeon();

            ResetSpawnRoom(PlacedRooms[0]);

            GenerateRandomDungeon();

            SetRoomBehavioursAndDecoration();

            SetBossRoom();

            SetDoors();

#if UNITY_EDITOR
            GetStatistic();
#endif

            GetComponent<Unity.AI.Navigation.NavMeshSurface>().BuildNavMesh();

            OnGenerationComplited();
        }

        private void GenerateRandomDungeon()
        {
            Vector2 currentPosition = new(PlacedRooms[0].coordinates.x, PlacedRooms[0].coordinates.y - 1);
            RoomScript lastPlacedRoom = new RoomScript();

            while (PlacedRooms.Where(x => x.CanHaveBehaviour).ToList().Count < GenerationParameters.numberOfRooms)
            {
                int x = Mathf.FloorToInt(currentPosition.x);
                int y = Mathf.FloorToInt(currentPosition.y);

                int xNext = random.Next(-1, 2);
                int yNext = random.Next(-1, 2);

                if (random.Next(3) == 0)
                    yNext = 0;
                else
                    xNext = 0;

                if (y >= 1 & y < Rooms.GetLength(1) - 1 & x >= 1 & x < Rooms.GetLength(0) - 1)
                {
                    if (Rooms[x, y] == null)
                    {
                        lastPlacedRoom = PlaceRoom(x, y); ;
                    }

                    currentPosition = new Vector2(x + xNext, y + yNext);
                }
                else
                {
                    currentPosition = lastPlacedRoom != null ? lastPlacedRoom.coordinates : new Vector2(x + xNext, y + yNext);
                }
            }
        }

        private void SetBossRoom()
        {
            var list = PlacedRooms.OrderBy(x => x.coordinates.y).ToList();

            int x = list[0].coordinates.x;
            int y = list[0].coordinates.y - 1;

            PlaceRoom(x, y, BossRoom);

            PlacedRooms[^1].transform.position += new Vector3(0, 0, 4);

            Rooms[x, y].gameObject.name = "BossRoom";
        }

        public RoomScript PlaceRoom(int x, int y, GameObject prefab = null)
        {
            if (!PlacedRooms.Where(i => i.coordinates == new Vector2(x, y)).Any())
            {
                GameObject room = null;

                if (prefab == null)
                {
                    prefab = RoomsPrefabs[random.Next(RoomsPrefabs.Count)].gameObject;

                    if (random.Next(3) == 0)
                    {
                        prefab = ConnectionRoomsPrefabs[random.Next(RoomsPrefabs.Count)].gameObject;
                    }
                }

                room = Instantiate(prefab, Dungeon.transform);

                RoomScript roomScript = room.GetComponent<RoomScript>();
                roomScript.coordinates = new Vector2Int(x, y);

                int roomX = x * Mathf.FloorToInt(roomScript.Size.x);
                int roomY = y * Mathf.FloorToInt(roomScript.Size.y);

                room.transform.position = new Vector3(roomX * TESTMODX, 0, roomY * TESTMODY);

                room.name = !roomScript.CanHaveBehaviour ? $"Connection {x} {y}" : $"Room {x} {y}";

                Rooms[x, y] = roomScript;

                PlacedRooms.Add(roomScript);

                return roomScript;
            }

            return null;
        }

        #region [Rooms Setup]
        public void SetRoomBehavioursAndDecoration()
        {
            var list = new List<RoomBehaviour>();

            foreach (var item in roomBehaviours)
            {
                int roomsAmount = (int)Math.Round(item.SpawnRate * GenerationParameters.numberOfEvents + 0.1);

                for (int i = 0; i < roomsAmount; i++)
                    list.Add(item);
            }

            behavioursBag = new SnuffleBag<RoomBehaviour>(list, random);

            for (int i = 1; i < PlacedRooms.Count; i++)
            {
                var room = PlacedRooms[i];

                if (room && room.CanHaveBehaviour)
                {
                    RoomBehaviour behaviour = behavioursBag.Next();

                    RoomScript[] neighbors = GetNeightbors(room.coordinates, behaviour.SameNeighborsRadius);

                    if (!behaviour.CanHaveSameNeighbors)
                    {
                        while (neighbors.Where(x => x && (x.Behaviour && x.Behaviour.BehaviourType == behaviour.BehaviourType)).ToList().Count != 0)
                        {
                            behaviour = behavioursBag.Next();

                            if (behaviour.CanHaveSameNeighbors)
                                break;
                        }
                    }

                    room.SetBehaviour(behaviour);
                    room.SetDecoration();
                }
            }
        }

        public void SetDoors()
        {
            foreach (var item in PlacedRooms)
                item.SetDoorways(GetNeighbors(item));
        }
        #endregion

        #region [Utilities]

        private RoomScript[] GetNeighboringRooms(Vector2Int coordinates)
        {
            List<RoomScript> neighbors = new List<RoomScript>(4);

            int[] IndexArray = new int[4] { 0, 1, 0, -1 };

            for (int i = 0; i < 4; i++)
            {
                RoomScript result = null;

                int x = IndexArray[i];
                int y = IndexArray[IndexArray.Length - 1 - i];

                int neighborX = coordinates.x + x;
                int neighborY = coordinates.y + y;

                if (neighborY >= 0 & neighborY < Rooms.GetLength(1)
                        & neighborX >= 0 & neighborX < Rooms.GetLength(0))
                {
                    result = Rooms[neighborX, neighborY];
                }

                neighbors.Add(result);
            }

            return neighbors.ToArray();
        }

        private RoomScript[] GetNeightbors(Vector2Int center, int radius)
        {
            var rooms = new List<RoomScript>();

            for (int y = -radius; y < radius + 1; y++)
            {
                for (int x = -radius; x < radius + 1; x++)
                {
                    int neighborY = center.y + y;
                    int neighborX = center.x + x;

                    if (neighborY >= 0 & neighborY < Rooms.GetLength(1)
                        & neighborX >= 0 & neighborX < Rooms.GetLength(0))
                    {
                        var room = Rooms[neighborX, neighborY];

                        if (room)
                        {
                            rooms.Add(room);
                        }
                    }
                }
            }

            return rooms.ToArray();
        }

        private bool[] GetNeighbors(RoomScript room)
        {
            var neighbors = new bool[4];
            var rooms = GetNeighboringRooms(room.coordinates);

            for (int i = 0; i < 4; i++)
                neighbors[i] = rooms[i] != null;

            return neighbors;
        }

        private void ResetSpawnRoom(RoomScript room)
        {
            room.coordinates = new Vector2Int(GenerationParameters.GridSize.x / 2, GenerationParameters.GridSize.y - 1);

            int xR = room.coordinates.x;
            int yR = room.coordinates.y;

            room.SetDoorways(new bool[] { false, false, false, false });

            room.transform.position = new Vector3(room.Size.x * xR * TESTMODX, 0, room.Size.y * yR * TESTMODY);

            Rooms[xR, yR] = room;

            transform.position = room.transform.position;
        }

        [ButtonMethod]
        public void ClearDungeon()
        {
            RoomScript room = PlacedRooms[0];

            if (PlacedRooms.Count > 1)
            {
                for (int i = 1; i < PlacedRooms.Count; i++)
                {
                    if (PlacedRooms[i].gameObject != null)
                    {
                        DestroyImmediate(PlacedRooms[i].gameObject);
                    }
                }
            }

            GetComponent<Unity.AI.Navigation.NavMeshSurface>().RemoveData();

            PlacedRooms.Clear();
            PlacedRooms.Add(room);

            ResetSpawnRoom(room);
        }

#if UNITY_EDITOR
        private void GetStatistic()
        {
            FreeChestAmount = PlacedRooms.Where(x => x && x.Behaviour && x.Behaviour.BehaviourType == RoomBehaviourType.FreeChest).Count();
            BuffsAmount = PlacedRooms.Where(x => x && x.Behaviour && x.Behaviour.BehaviourType == RoomBehaviourType.Buffer).Count();
            EnemiesAmount = PlacedRooms.Where(x => x && x.Behaviour && x.Behaviour.BehaviourType == RoomBehaviourType.Battle).Count();
            FontainesAmount = PlacedRooms.Where(x => x && x.Behaviour && x.Behaviour.BehaviourType == RoomBehaviourType.Heal).Count();

            BehaviourAmount = FreeChestAmount + BuffsAmount + EnemiesAmount + FontainesAmount;
        }
#endif

        #endregion
    }
}