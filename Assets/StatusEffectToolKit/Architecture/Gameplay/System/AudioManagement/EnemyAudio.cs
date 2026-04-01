/*
 * Description: Handles playing audio clips for the enemy 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AudioManagement
{
    public class EnemyAudio : MonoBehaviour
    {
        [SerializeField]
        private List<AudioClip> clips = new List<AudioClip>();
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private float playClipWaitTime;

        private WaitForSeconds clipWaitEnumerator;

        private void Start()
        {
            clipWaitEnumerator = new WaitForSeconds(playClipWaitTime);
            StartCoroutine(ChooseRandomClip());
        }

        /// <summary>
        /// Choose a random clip to give enemies variation
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChooseRandomClip()
        {
            yield return clipWaitEnumerator;
            int randomClip = Random.Range(0, clips.Count);
            AudioManager.Instance.PlayOneShotClip(clips[randomClip], audioSource);
            StartCoroutine(ChooseRandomClip());

        }
    }
}