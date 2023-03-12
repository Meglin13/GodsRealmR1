using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;

public class DungeonGeneratorScript : MonoBehaviour
{
    public List<GameObject> RoomsPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshBuilder.ClearAllNavMeshes();
        NavMeshBuilder.BuildNavMesh();
    }

    public void GenerateDungeon()
    {

    }

#if UNITY_EDITOR

    [ButtonMethod]
    public void BakeSurface()
    {
        NavMeshBuilder.ClearAllNavMeshes();
        NavMeshBuilder.BuildNavMesh();
    }
#endif
}
