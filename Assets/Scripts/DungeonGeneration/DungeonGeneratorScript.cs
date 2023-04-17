using MyBox;
using MyBox.EditorTools;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

[Serializable]
public class GenerationParameters
{
    public Vector2Int GridSize;

    public int numberOfRooms = 5;
    public int numberOfEvents;

    public int ChestRooms;
    public int BattleRooms;
    public int BuffRooms;

    public int Seed = 0;
}


public class DungeonGeneratorScript : MonoBehaviour
{
    public GenerationParameters GenerationParameters = new GenerationParameters();
    public int SeedOutput;

    public RoomScript[,] Rooms;

    public List<RoomScript> RoomsPrefabs = new List<RoomScript>();
    public List<RoomScript> PlacedRooms = new List<RoomScript>();
    public GameObject Dungeon;

    public Random random;

    public int TESTMODX = 1;
    public int TESTMODY = -1;

    public List<RoomBehaviour> roomBehaviours;
    public SnuffleBag<RoomBehaviour> behavioursBag;

    public event Action OnGenerationComplited = delegate { };

    private void Start()
    {
        if (RunManager.Params.GenerationParameters != null)
        {
            Debug.Log("Bruh");
            GenerationParameters = RunManager.Params.GenerationParameters;
        }

        GenerateDungeon();

        foreach (var item in GameManager.Instance.partyManager.PartyMembers)
        {
            item.transform.position = PlacedRooms[0].transform.position;
            item.GetComponent<NavMeshAgent>().enabled = false;
            item.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    [ButtonMethod]
    private void GenerateDungeon()
    {
        var seed = GenerationParameters.Seed == 0
            ? UnityEngine.Random.Range(int.MinValue, int.MaxValue)
            : GenerationParameters.Seed;

        SeedOutput = seed;

        random = new Random(seed);

        Rooms = new RoomScript[GenerationParameters.GridSize.x, GenerationParameters.GridSize.y];

        ClearDungeon();

        GenerateRandomDungeon();

        SetBossRoom();

        SetDoors();

        SetRoomBehaviours();

        GetComponent<Unity.AI.Navigation.NavMeshSurface>().BuildNavMesh();

        OnGenerationComplited();
    }

    private void GenerateRandomDungeon()
    {
        Vector2 currentPosition = new(PlacedRooms[0].coordinates.x, PlacedRooms[0].coordinates.y - 1);
        RoomScript lastPlacedRoom = new RoomScript();

        int i = 0;

        while (i < GenerationParameters.numberOfRooms)
        {
            int x = Mathf.FloorToInt(currentPosition.x);
            int y = Mathf.FloorToInt(currentPosition.y);

            int xNext = random.Next(-1, 2);
            int yNext = random.Next(-1, 2);

            if (random.Next(0, 3) == 0)
                yNext = 0;
            else
                xNext = 0;

            if (y >= 1 & y < Rooms.GetLength(1) - 1 & x >= 1 & x < Rooms.GetLength(0) - 1)
            {
                if (Rooms[x, y] == null)
                {
                    PlaceRoom(x, y);
                    lastPlacedRoom = Rooms[x, y];
                    i++;
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

        PlaceRoom(x, y);

        Rooms[x, y].gameObject.name = "BossRoom";
        Rooms[x, y].gameObject.SetEditorIcon(true, 5);
    }

    private void ResetSpawnRoom(RoomScript room)
    {
        room.coordinates = new Vector2Int(GenerationParameters.GridSize.x / 2, GenerationParameters.GridSize.y - 1);

        int xR = room.coordinates.x;
        int yR = room.coordinates.y;

        room.SetDoorways(new bool[] { false, false, false, false });

        Rooms[xR, yR] = room;

        room.transform.position = new Vector3(room.Size.x * xR * TESTMODX, 0, room.Size.y * yR * TESTMODY);

        this.transform.position = room.transform.position;
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

    public void PlaceRoom(int x, int y)
    {
        if (PlacedRooms.Where(i => i.coordinates == new Vector2(x, y)).FirstOrDefault() == null)
        {
            GameObject room = Instantiate(RoomsPrefabs[UnityEngine.Random.RandomRange(0, RoomsPrefabs.Count - 1)].gameObject, Dungeon.transform);

            RoomScript roomScript = room.GetComponent<RoomScript>();
            roomScript.coordinates = new Vector2Int(x, y);

            int roomX = x * Mathf.FloorToInt(roomScript.Size.x);
            int roomY = y * Mathf.FloorToInt(roomScript.Size.y);

            room.transform.position = new Vector3(roomX * TESTMODX, 0, roomY * TESTMODY);
            room.name = $"Room {roomScript.coordinates.x} {roomScript.coordinates.y}";

            Rooms[x, y] = roomScript;

            PlacedRooms.Add(roomScript);
        }
    }

    public void SetRoomBehaviours()
    {
        behavioursBag = new SnuffleBag<RoomBehaviour>(random);



    }

    public void SetDoors()
    {
        foreach (var item in PlacedRooms)
        {
            item.SetDoorways(GetNeighbors(item.coordinates));
        }
    }

    public bool[] GetNeighbors(Vector2 roomIndex)
    {
        List<bool> neighbors = new List<bool>(4);

        int[] IndexArray = new int[4] { 0, 1, 0, -1 };

        for (int i = 0; i < 4; i++)
        {
            bool result = false;

            int x = IndexArray[i];
            int y = IndexArray[IndexArray.Length - 1 - i];

            int neighborX = Mathf.FloorToInt(roomIndex.x) + x;
            int neighborY = Mathf.FloorToInt(roomIndex.y) + y;

            if (neighborY >= 0 & neighborY < Rooms.GetLength(1)
                    & neighborX >= 0 & neighborX < Rooms.GetLength(0))
            {
                if (Rooms[neighborX, neighborY] != null)
                {
                    result = true;
                }
            }

            neighbors.Add(result);
        }

        return neighbors.ToArray();
    }
}