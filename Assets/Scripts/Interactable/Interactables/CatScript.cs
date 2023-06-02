using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactables
{
    [RequireComponent(typeof(Animator), typeof(AudioSource))]
    public class CatScript : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private AudioClip[] audioClips;

        private Animator animator;
        private AudioSource audioSource;
        private float TimePassed;
        [SerializeField]
        private float TimeBetweenActions;

        public bool CanInteract() => true;

        public void Interaction()
        {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            TimePassed += Time.deltaTime;

            if (TimePassed > TimeBetweenActions)
            {
                animator.SetTrigger(Random.Range(0, 2).ToString());

                TimePassed = 0;
            }
        }
    }
}
