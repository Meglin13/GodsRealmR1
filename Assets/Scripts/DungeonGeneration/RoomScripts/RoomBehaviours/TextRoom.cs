using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace DungeonGeneration
{
    [CreateAssetMenu(fileName = "Text Room", menuName = "Objects/Dungeon Generator/RoomBehaviour/Text Room")]
    public class TextRoom : RoomBehaviour
    {
        private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        [SerializeField, Range(0f, 5f)]
        private float fadeDuration = 1f;

        public TextRoom() => BehaviourType = RoomBehaviourType.Misc;

        public override void Initialize(RoomScript room)
        {
            base.Initialize(room);

            texts.Clear();

            foreach (var item in room.Props)
            {
                if (item.TryGetComponent(out TextMeshProUGUI text))
                {
                    texts.Add(text);

                    Color targetColor = text.color;
                    targetColor.a = 0;

                    text.color = targetColor;

                    text.gameObject.SetActive(false);
                    text.gameObject.SetActive(true);
                }
            }
        }

        public override void OnRoomEnter()
        {
            Initialize(Room);

            foreach (var text in texts)
                Room.StartCoroutine(ShowText(text, false));
        }

        public override void OnRoomExit()
        {
            foreach (var text in texts)
                Room.StartCoroutine(ShowText(text, true));
        }

        public IEnumerator ShowText(TextMeshProUGUI text, bool MakeAlpha)
        {
            float elapsedTime = 0f;
            Color initialColor = text.color;

            Color targetColor = text.color;
            targetColor.a = MakeAlpha ? 0 : 1;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                text.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
                yield return null;
            }
        }
    }
}
