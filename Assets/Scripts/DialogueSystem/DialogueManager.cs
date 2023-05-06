using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class DialogueManager : UIScript, ISingle
    {
        #region Singleton

        public static DialogueManager Instance;
        public void Initialize()
        {
            Instance = this;
        }

        #endregion

        [SerializeField]
        private float TextSpeed;

        private Dialogue Dialogue;
        private int CurrentLineIndex;
        private UnityEvent onDialogueEnd;

        private Label SpeakerLB;
        private Label ReplicaLB;

        private VisualElement LeftSpeaker;
        private VisualElement RightSpeaker;

        private VisualElement DialoguePanel;
        private Clickable nextLineClickabel;

        private bool IsTyping = false;
        private string CurrentLine;

        private Clickable SkipManupulator;

        internal override void OnBind()
        {
            base.OnBind();
            
            SkipManupulator ??= new Clickable(EndDialogue);

            SpeakerLB = root.Q<Label>("SpeakerLB");
            ReplicaLB = root.Q<Label>("ReplicaLB");

            var SkipLB = root.Q<Label>("SkipLB");
            SkipLB.RemoveManipulator(SkipManupulator);
            SkipLB.AddManipulator(SkipManupulator);

            LeftSpeaker = root.Q<VisualElement>("LeftSpeaker");
            RightSpeaker = root.Q<VisualElement>("RightSpeaker");

            DialoguePanel = root.Q<VisualElement>("DialoguePanel");
            DialoguePanel.AddManipulator(nextLineClickabel);
        }

        private void Awake()
        {
            Initialize();
            nextLineClickabel = new Clickable(NextLine);
            gameObject.SetActive(false);
        }

        public void StartDialogue(Dialogue dialogue, UnityEvent onDialogueEnd = null)
        {
            UIManager.Instance.OpenMenu(gameObject, false);

            this.onDialogueEnd = onDialogueEnd;

            Dialogue = dialogue;
            PartyManager.Instance.GetPlayer().playerInput.enabled = false;

            CurrentLineIndex = 0;
            StartCoroutine(TypeLine());
        }

        private void NextLine()
        {
            if (IsTyping)
            {
                IsTyping = false;
                ReplicaLB.text = CurrentLine;
            }

            StopAllCoroutines();

            if (CurrentLineIndex < Dialogue.replicas.Count - 1)
            {
                CurrentLineIndex++;
                StartCoroutine(TypeLine());
            }
            else
            {
                EndDialogue();
            }
        }

        private void SetSpeakersSprites()
        {
            var replica = Dialogue.replicas[CurrentLineIndex];
            var leftSpeaker = Dialogue.Speakers[0];

            LeftSpeaker.style.backgroundImage = new StyleBackground(leftSpeaker.Idle);

            if (replica.Speaker != leftSpeaker)
            {
                RightSpeaker.style.backgroundImage = new StyleBackground(Dialogue.replicas[CurrentLineIndex].Speaker.Idle);
            }

            VisualElement speakerImage = LeftSpeaker;
            VisualElement listenerImage = RightSpeaker;

            if (replica.Speaker != leftSpeaker)
            {
                speakerImage = RightSpeaker;
                listenerImage = LeftSpeaker;
            }

            speakerImage.AddToClassList("speaking-character");
            speakerImage.RemoveFromClassList("listening-character");

            listenerImage.AddToClassList("listening-character");
            listenerImage.RemoveFromClassList("speaking-character");
        }

        private IEnumerator TypeLine()
        {
            IsTyping = true;

            ReplicaLB.text = string.Empty;
            var replica = Dialogue.replicas[CurrentLineIndex];

            SetSpeakersSprites();

            manager.ChangeLabelsText(SpeakerLB, replica.Speaker.Name, CharacterTable);

            CurrentLine = manager.GetLocalizedString(replica.Line, DialogueTable);

            foreach (var c in CurrentLine)
            {
                ReplicaLB.text += c;
                yield return new WaitForSecondsRealtime(TextSpeed);
            }

            yield return new WaitForSecondsRealtime(TextSpeed);

            IsTyping = false;
        }

        private void EndDialogue()
        {
            manager.GoBack();
            Dialogue.IsUnlocked = true;
            PartyManager.Instance.GetPlayer().playerInput.enabled = true;

            onDialogueEnd?.Invoke();
        }
    }
}
