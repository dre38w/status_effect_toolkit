
/*
Description: Handles logic for when the player experiences vision blur
*/

using Service.Framework.StatusSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gameplay.System.StatusSystem
{
    public class Freeze_StatusEffect : StatusEffect
    {
        [SerializeField]
        private ScriptableRendererFeature fullScreenFrozen;
        [SerializeField]
        private Material freezeMaterial;

        private int vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");

        private void Awake()
        {
            fullScreenFrozen.SetActive(false);
            freezeMaterial.SetFloat(vignetteIntensity, 0);
        }

        public override void HandleRunStart()
        {
            fullScreenFrozen.SetActive(true);
            freezeMaterial.SetFloat(vignetteIntensity, 2);
            SetState(StatusEffectState.Running);
        }

        public override void StatusEffectEnding(float deltaTime)
        {
            fullScreenFrozen.SetActive(false);
            freezeMaterial.SetFloat(vignetteIntensity, 0);

            //end condition met, effect is now done
            if (freezeMaterial.GetFloat(vignetteIntensity) == 0)
            {
                SetState(StatusEffectState.Ended);
            }
        }

        public override void StatusEffectEnded()
        {
            base.StatusEffectEnded();
            fullScreenFrozen.SetActive(false);
            freezeMaterial.SetFloat(vignetteIntensity, 0);

        }
    }
}