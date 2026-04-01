/*
 * Description: Plays a sfx when the freeze effect starts
 */

using Gameplay.System.StatusSystem;
using Service.Framework.StatusSystem;
using UnityEngine;

namespace Gameplay.AudioManagement
{
    public class StartFreezeEffectAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip clip;

        [SerializeField]
        private Freeze_StatusEffect freezeEffect;

        private void Start()
        {
            freezeEffect.OnStatusEffectStarted.AddListener(PlayAudioClip);
        }

        private void PlayAudioClip(StatusEffect effect)
        {
            //make sure this is the freeze effect
            if (effect == freezeEffect)
            {
                AudioManager.Instance.PlayOneShotClip(clip);
            }
        }

        private void OnDestroy()
        {
            freezeEffect.OnStatusEffectStarted.RemoveListener(PlayAudioClip);

        }
    }
}