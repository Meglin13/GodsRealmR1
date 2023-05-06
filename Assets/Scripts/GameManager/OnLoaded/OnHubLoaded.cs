using UnityEngine;

namespace OnLoaded
{
    internal class OnHubLoaded : MonoBehaviour
    {
        private void Awake()
        {
            SaveLoadSystem.SaveLoadSystem.Save();
        }
    }
}