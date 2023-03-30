using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = System.Random;

public class DungeonGeneratorScript : MonoBehaviour
{
    public int GridSizeX = 7;
    public int GridSizeY;

    public RoomScript[,] Rooms;

    public List<RoomScript> RoomsPrefabs = new List<RoomScript>();
    public List<RoomScript> PlacedRooms = new List<RoomScript>();
    public GameObject Dungeon;

    public int numberOfRooms = 5;

    public int Seed;
    public Random random;

    public int TESTMODX = 1;
    public int TESTMODY = -1;

    public event Action OnGenerationComplited = delegate { };

    // Start is called before the first frame update
    private void Start()
    {
        GenerateDungeon();

        foreach (var item in GameManager.Instance.partyManager.PartyMembers)
        {
            item.transform.position = PlacedRooms[0].transform.position;
        }
    }

    [ButtonMethod]
    private void GenerateDungeon()
    {
        random = Seed != 0 ? (new(Seed)) : (new());

        GridSizeY = Mathf.FloorToInt(GridSizeX * 1.5f);

        Rooms = new RoomScript[GridSizeX, GridSizeY];

        ClearDungeon();

        GenerateRandomDungeon();

        SetBossRoom();

        SetDoors();

        GetComponent<NavMeshSurface>().BuildNavMesh();

        OnGenerationComplited();
    }

    private void GenerateRandomDungeon()
    {
        Vector2 currentPosition = new(PlacedRooms[0].coordinates.x, PlacedRooms[0].coordinates.y - 1);
        RoomScript lastPlacedRoom = new RoomScript();

        int i = 0;

        while (i < numberOfRooms)
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
        int y = 0;
        for (int x = 0; x < Rooms.GetLength(0); x++)
        {
            if (Rooms[x, y] == null)
            {
                PlaceRoom(x, 0);
            }

            if (x == Rooms.GetLength(0) - 1)
            {
                y = 0;
                x = 0;
            }
        }
    }

    private void ResetSpawnRoom(RoomScript room)
    {
        room.coordinates = new Vector2(Mathf.FloorToInt(GridSizeX / 2), GridSizeY - 1);

        int xR = Mathf.FloorToInt(room.coordinates.x);
        int yR = Mathf.FloorToInt(room.coordinates.y);

        room.SetDoorways(new bool[] { false, false, false, false });

        Rooms[xR, yR] = room;

        room.transform.position = new Vector3(room.Size.x * xR * TESTMODX, 0, room.Size.y * yR * TESTMODY);

        this.transform.position = room.transform.position;
    }

    [ButtonMethod]
    public void ClearDungeon()
    {
        RoomScript room = PlacedRooms[0];

        for (int i = 1; i < PlacedRooms.Count; i++)
        {
            if (PlacedRooms[i].gameObject != null)
            {
                DestroyImmediate(PlacedRooms[i].gameObject);
            }
        }

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
            roomScript.coordinates = new Vector2(x, y);

            int roomX = x * Mathf.FloorToInt(roomScript.Size.x);
            int roomY = y * Mathf.FloorToInt(roomScript.Size.y);

            room.transform.position = new Vector3(roomX * TESTMODX, 0, roomY * TESTMODY);
            room.name = $"Room {roomScript.coordinates.x} {roomScript.coordinates.y}";

            Rooms[x, y] = roomScript;

            PlacedRooms.Add(roomScript);
        }
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