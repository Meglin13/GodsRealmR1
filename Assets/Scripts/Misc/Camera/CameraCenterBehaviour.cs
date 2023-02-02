using UnityEngine;

public class CameraCenterBehaviour : MonoBehaviour
{
    private GameObject target;

    private void Start()
    {
        target = SetTarget();
        transform.SetParent(null);
    }

    private void LateUpdate()
    {
        target = SetTarget();

        if (target)
        {
            transform.position = target.transform.position;

            gameObject.transform.SetParent(target.transform);

            gameObject.transform.rotation = gameObject.transform.parent.rotation;
        }
    }

    private GameObject SetTarget()
    {
        CharacterScript[] targets = GameObject.FindObjectsOfType<CharacterScript>();

        foreach (var character in targets)
        {
            if (character.IsActive)
                return character.gameObject;
        }
        return null;
    }
}