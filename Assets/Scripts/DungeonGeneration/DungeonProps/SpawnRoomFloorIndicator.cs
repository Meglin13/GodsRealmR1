using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace DungeonGeneration.DungeonProps
{
    public class SpawnRoomFloorIndicator : MonoBehaviour
    {
        [SerializeField] private LocalizedString localString;
        [SerializeField] private TextMeshProUGUI text;

        private void OnEnable()
        {
            localString.Arguments = new object[] { RunManager.CurrentFloor };
            localString.StringChanged += UpdateString;
        }

        private void OnDisable()
        {
            localString.StringChanged -= UpdateString;
        }

        private void UpdateString(string value)
        {
            text.text = value;
        }
    }
}
