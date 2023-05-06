using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour, ISingle
{
    public GameObject MarkerPrefab;

    public static MiniMapManager Instance { get; private set; }

    [ContextMenu("InitializeSettings Minimap Markers")]
    public void Initialize()
    {
        DeleteMarkers();

        Color EnemyMarkerColor = GameManager.Instance.colorManager.EnemyMarkerColor;
        Color CharMarkerColor = GameManager.Instance.colorManager.CharMarkerColor;

        EnemyMarkerColor = EnemyMarkerColor != Color.black ? EnemyMarkerColor : Color.red;
        CharMarkerColor = CharMarkerColor != Color.black ? CharMarkerColor : Color.green;

        EnemyMarkerColor.a = 1f;
        CharMarkerColor.a = 1f;

        List<GameObject> enemies = GameObject.FindGameObjectsWithTag(AIUtilities.EnemyTag).ToList();
        List<GameObject> characters = GameObject.FindGameObjectsWithTag(AIUtilities.CharsTag).ToList();

        GenerateMarkersForGroup(enemies, EnemyMarkerColor);
        GenerateMarkersForGroup(characters, CharMarkerColor);
    }

    private void GenerateMarkersForGroup(List<GameObject> gameObjects, Color MarkerColor)
    {
        GameObject Prefab = MarkerPrefab;

        Prefab.GetComponentInChildren<Image>().color = MarkerColor;

        foreach (var item in gameObjects)
        {
            GameObject Marker = GameObject.Instantiate(Prefab, item.transform, false);
            Marker.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    [ContextMenu("Delete Minimap Markers")]
    public void DeleteMarkers()
    {
        List<GameObject> Markers = GameObject.FindGameObjectsWithTag("Marker").ToList();

        if (Markers.Count > 0)
        {
            for (int i = 0; i < Markers.Count; i++)
            {
                DestroyImmediate(Markers[i]);
            }
        }
    }
}