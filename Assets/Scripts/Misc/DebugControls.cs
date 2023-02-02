using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugControls : MonoBehaviour
{
    public GameObject Skeleton;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();

        var spawn = playerInput.actions["Spawn Enemy"];
        spawn.performed += SpawnEnemy;

        var restart = playerInput.actions["Restart"];
        restart.performed += ctx => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SpawnEnemy(InputAction.CallbackContext obj)
    {
        Transform spawnpoint = GameManager.Instance.partyManager.PartyMembers[GameManager.Instance.partyManager.LeaderIndex].transform;
        Instantiate(Skeleton, spawnpoint.position, spawnpoint.rotation);
    }
}
