using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{

    // Put the gras prefabs here. They will be chosen by random
    public List<GameObject> grassPrefabs = new List<GameObject>();
    public int grassNumber = 64;
    public float grassAreaWidth = 5;
    public float grassAreaHeight = 5;
    public string interactionTag = "target"; // Tag objects with this string, that you want to interact with the gras

    private Vector4[] grassInteractionPositions = new Vector4[4];
    private Transform ground;
    private List<GameObject> grass = new List<GameObject>();

    void Awake()
    {
        ground = transform;
        float groundWidthHalf = grassAreaWidth / 2;
        float groundDepthHalf = grassAreaHeight / 2;

        // Create some gras at random positions in given area
        for (int grassIndex = 0; grassIndex < grassNumber; grassIndex++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-groundWidthHalf, groundWidthHalf), 0, Random.Range(-groundDepthHalf, groundDepthHalf));
            GameObject newGrass = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Count)], position, Quaternion.Euler(0, Random.Range(0, 360), 0), ground.transform);
            // newGrass.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            grass.Add(newGrass);
        }
    }

    private void Update()
    {
        int interactionObjIndex = 0;
        foreach (GameObject interactionObj in GameObject.FindGameObjectsWithTag(interactionTag))
        {
            grassInteractionPositions[interactionObjIndex++] = interactionObj.transform.position + new Vector3(0, 0.5f, 0);
        }
        Shader.SetGlobalFloat("_PositionArray", interactionObjIndex);
        Shader.SetGlobalVectorArray("_Positions", grassInteractionPositions);
    }
}
