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

        var FPS = playerInput.actions["SetFPS"];
        FPS.performed += ctx =>
        {
            Application.targetFrameRate = 300;
        };
    }

    private void SpawnEnemy(InputAction.CallbackContext obj)
    {
        Transform spawnpoint = GameManager.Instance.partyManager.PartyMembers[GameManager.Instance.partyManager.LeaderIndex].transform;
        Instantiate(Skeleton, spawnpoint.position, spawnpoint.rotation);
    }
}
