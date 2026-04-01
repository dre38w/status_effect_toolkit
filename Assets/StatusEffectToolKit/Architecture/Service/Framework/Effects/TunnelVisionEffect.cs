/*
 * Description: An example script to increase/descrease tunnel vision post processing effect
 */

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Service.Framework.Effects
{
    public class TunnelVisionEffect : MonoBehaviour
    {
        [SerializeField]
        private Volume volume;
        private Vignette vignette;

        public float TunnelIntensity { get; private set; }
        public float TunnelSmoothness { get; private set; }

        private void Start()
        {
            volume.profile.TryGet(out vignette);

            TunnelIntensity = vignette.intensity.value;
            TunnelSmoothness = vignette.smoothness.value;
        }

        /// <summary>
        /// Update the vignette values
        /// </summary>
        /// <param name="intensityValue"></param>
        /// <param name="smoothnessValue"></param>
        public void SetTunnelStrength(float intensityValue, float smoothnessValue)
        {
            vignette.intensity.value = Mathf.Clamp01(intensityValue);
            vignette.smoothness.value = Mathf.Clamp01(smoothnessValue);
        }

        public void ResetEffect()
        {
            vignette.intensity.value = TunnelIntensity;
            vignette.smoothness.value = TunnelSmoothness;
        }
    }
}