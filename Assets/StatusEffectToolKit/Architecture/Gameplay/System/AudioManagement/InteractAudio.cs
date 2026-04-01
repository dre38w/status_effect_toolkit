/*
 * Description: Play a sfx when interacting with something 
 */

using Gameplay.System;
using Service.Framework;
using UnityEngine;

namespace Gameplay.AudioManagement
{
    public class InteractAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip clip;
        [SerializeField]
        private AudioSource audioSource;

        private PlayerInteractor interactor;

        private void Start()
        {
            interactor = ReferenceRegistry.Instance.Player.GetComponent<PlayerInteractor>();
            interactor.OnInteracted.AddListener(PlayInteractClip);
        }

        private void PlayInteractClip()
        {
            AudioManager.Instance.PlayOneShotClip(clip, audioSource);
        }

        private void OnDestroy()
        {
            interactor.OnInteracted.RemoveListener(PlayInteractClip);

        }
    }
}