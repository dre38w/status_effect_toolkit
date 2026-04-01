/*
 * Description: Generic audio manager that handles playing audio clips
 */

using UnityEngine;

namespace Gameplay.AudioManagement
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField]
        private AudioSource defaultAudioSource;
        private AudioSource activeAudioSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// Play the one shot audio clip
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="source"></param>
        public void PlayOneShotClip(AudioClip clip, AudioSource source = null)
        {
            //if nothing was passed, use the default for global sound
            if (source == null)
            {
                activeAudioSource = defaultAudioSource;
            }
            //if source should be on a specific object
            else
            {
                activeAudioSource = source;
            }
            activeAudioSource.clip = clip;
            activeAudioSource.PlayOneShot(clip);
        }
    }
}