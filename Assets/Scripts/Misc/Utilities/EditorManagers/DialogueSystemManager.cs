using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueSystemManager", menuName = "Objects/TestingUtilities/DialogueSystemManager")]
    public class DialogueSystemManager : ScriptableObject
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            Dialogues = Resources.LoadAll<Dialogue>("ScriptableObjects/Dialogues").ToList();
            Speakers = Resources.LoadAll<Speaker>("ScriptableObjects/Dialogues/Speakers").ToList();
        }
#endif

        public List<Speaker> Speakers;
        public List<Dialogue> Dialogues;
    }
}