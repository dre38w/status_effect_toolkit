
/*
Description: Example script for tunnel vision
*/

using UnityEngine;
using Service.Framework.StatusSystem;
using Service.Framework.Effects;

namespace Gameplay.System.StatusSystem
{
    public class TunnelVision_StatusEffect : StatusEffect
    {
        [SerializeField]
        private TunnelVisionEffect effect;
        [SerializeField]
        private float tunnelIntensity = 0.02f;
        [SerializeField]
        private float tunnelSmoothness = 0.5f;

        private float currentTunnelIntensity;

        public override void HandleRunStart()
        {
            SetState(StatusEffectState.Running);
        }

        public override void HandleStatusEffectRunning(float deltaTime)
        {
            //increase the intensity of the volume's vignette intensity
            currentTunnelIntensity += deltaTime * tunnelIntensity;
            currentTunnelIntensity = Mathf.Clamp01(currentTunnelIntensity);
            effect.SetTunnelStrength(currentTunnelIntensity, tunnelSmoothness);
        }

        public override void StatusEffectEnding(float deltaTime)
        {
            //gradually decrease the vignette's intensity
            currentTunnelIntensity -= deltaTime * tunnelIntensity;
            currentTunnelIntensity = Mathf.Clamp(currentTunnelIntensity, 0, 1);
            effect.SetTunnelStrength(currentTunnelIntensity, tunnelSmoothness);

            //end condition met, effect is now done
            if (currentTunnelIntensity == 0)
            {
                SetState(StatusEffectState.Ended);
            }
        }

        public override void StatusEffectEnded()
        {
            base.StatusEffectEnded();
            currentTunnelIntensity = 0;
            effect.ResetEffect();
            
        }
    }
}