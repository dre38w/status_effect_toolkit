/*
 * Description: Example script that handles light mechanics.  Affects status level, handles toggling on/off, etc. 
 */

using Gameplay.System.StatusSystem.Actions;
using Service.Core;
using Service.Framework.StatusSystem;
using System.Collections;
using UnityEngine;

namespace Gameplay.System
{
    public class LightExample : MonoBehaviour, IInteractable
    {
        private bool canInteractWithLight;
        [SerializeField]
        private Light lightComponent;
        [SerializeField]
        private float activeLightIntensity = 800f;
        [SerializeField]
        private ActivateObjectTrigger activateLightTrigger;

        private HandleDarknessAction handleDarknessAction;

        private void Start()
        {
            lightComponent.intensity = 0;
            activateLightTrigger.OnEnteredTriggerObject.AddListener(OnLightInteractable);
            activateLightTrigger.OnExitedTriggerObject.AddListener(OnLightNotInteractable);

            StartCoroutine(RegisterLight());
        }

        private IEnumerator RegisterLight()
        {
            //wait a frame to allow the singleton managers time to initialize
            yield return null;
            handleDarknessAction = (HandleDarknessAction)StatusManager.Instance.ActionHandlers.Find(a => a is HandleDarknessAction);
            handleDarknessAction.RegisterLight(lightComponent);
        }

        public void Interact(GameObject interactor)
        {
            if (!canInteractWithLight)
            {
                return;
            }
            ToggleLight();
        }

        private void ToggleLight()
        {
            //turn on
            if (Mathf.Approximately(lightComponent.intensity, 0))
            {
                lightComponent.intensity = activeLightIntensity;

                //safe in the light, so decrease a status level and immediately clear up the status effects
                if (StatusLevelAdjuster.Instance.GetStatusTrackingState())
                {
                    StatusLevelAdjuster.Instance.StopTrackingStatus();
                }
                StatusLevelAdjuster.Instance.DecreaseStatusLevel();
                StatusEffectManager.Instance.EndAllStatusEffects();
            }
            //turn off
            else
            {
                lightComponent.intensity = 0;
            }
        }

        private void OnLightInteractable()
        {
            ReferenceRegistry.Instance.MainUi.SetContextualUiVisible(true);
            canInteractWithLight = true;
        }

        private void OnLightNotInteractable()
        {
            ReferenceRegistry.Instance.MainUi.SetContextualUiVisible(false);
            canInteractWithLight = false;
        }

        private void OnDestroy()
        {
            handleDarknessAction.UnRegisterLight(lightComponent);

            activateLightTrigger.OnEnteredTriggerObject.RemoveListener(OnLightInteractable);
            activateLightTrigger.OnExitedTriggerObject.RemoveListener(OnLightNotInteractable);

        }
    }
}