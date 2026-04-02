
/*
Description: Manages selecting, starting, stopping, and running the status effects
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Service.Framework.Tools;

using Random = UnityEngine.Random;

namespace Service.Framework.StatusSystem
{
    public class StatusEffectManager : MonoBehaviour
    {
        public class OnStatusEffectSelectedEvent : UnityEvent<StatusEffectsData> { }
        public OnStatusEffectSelectedEvent OnStatusEffectSelected = new OnStatusEffectSelectedEvent();
        [HideInInspector]
        public UnityEvent OnAllStatusEffectsEnded = new UnityEvent();

        public static StatusEffectManager Instance;

        /// <summary>
        /// The data used when a Status Level changes
        /// </summary>
        [Serializable]
        public class ConfigFields
        {
            public bool requireWaitTimeBetweenEffects = false;
            public bool isRandomTimeToSelectEffects = false;
            [Tooltip("The wait time between changing from one stats effect to another." +
                "Useful if there is any transition logic to be done before starting the new effect.")]
            public float timeToSelectEffect = 10.0f;
            public float maxTimeToSelectEffect;
            public float minTimeToSelectEffect;
        }
        private ConfigFields configFields;
        private float currentTimeToSelectEffect;

        /// <summary>
        /// The config data for the on the overall Status Level including the Status Effect data
        /// </summary>
        private StatusEffectSettings.StatusLevelConfig currentStatusEffectConfig;

        /// <summary>
        /// List of all status effects
        /// </summary>
        private List<StatusEffect> statusEffects = new List<StatusEffect>();

        private Coroutine statusEffectSelectionCoroutine;
        private Coroutine statusLevelPeriodicCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            configFields = StatusManager.Instance.StatusSettings.statusEffectManagerConfig;
            if (configFields == null)
            {
                Debug.LogError("StatusEffectSettings.statusEffectManagerConfig is null.  This must be populated.");
            }
            //passed when the status level changes.
            StatusManager.Instance.OnStatusLevelChanged.AddListener(SelectStatusEffect);
            //add a listener for each status effect
            foreach (StatusEffect effect in statusEffects)
            {
                effect.OnStatusEffectEnded.AddListener(StatusEffectEnded);
            }
        }

        public static void AddStatusEffect(StatusEffect effect)
        {
            if (!Instance.statusEffects.Contains(effect))
            {
                Instance.statusEffects.Add(effect);
            }
        }

        public static void RemoveStatusEffect(StatusEffect effect)
        {
            if (Instance.statusEffects.Contains(effect))
            {
                Instance.statusEffects.Remove(effect);
            }
        }

        private void Update()
        {
            if (statusEffects.Count > 0)
            {
                for (int i = 0; i < statusEffects.Count; i++)
                {
                    statusEffects[i].StatusEffectUpdate(Time.deltaTime);
                }
            }
        }

        /// <summary>
        /// Status level changed so we need to update the selected status effect
        /// </summary>
        /// <param name="level"></param>
        /// <param name="effect">The SO reference that was selected via the StatusManager</param>
        public void SelectStatusEffect(int level, StatusEffectSettings.StatusLevelConfig effect)
        {
            currentStatusEffectConfig = effect;

            CancelPeriodicEffectSelection();

            //end the status effect that were running from a previous status level
            //so we can start with the status effect associated with the new status level
            StartEndingAllStatusEffects();

            if (!configFields.requireWaitTimeBetweenEffects)
            {
                SetCurrentStatusEffect();
                StartPeriodicSelection();
                return;
            }
            if (configFields.isRandomTimeToSelectEffects)
            {
                currentTimeToSelectEffect = SelectStatusEffectTimer();
            }
            else
            {
                currentTimeToSelectEffect = configFields.timeToSelectEffect;
            }

            if (statusEffectSelectionCoroutine == null)
            {
                //use a timer in the event we want to ease into the status effect selection
                statusEffectSelectionCoroutine = StartCoroutine(TimeToSelectStatusEffect(effect));
            }
        }

        /// <summary>
        /// Begin the periodic effect selection process
        /// </summary>
        private void StartPeriodicSelection()
        {
            if (!currentStatusEffectConfig.isPeriodicStatusLevel)
            {
                return;
            }

            if (statusLevelPeriodicCoroutine != null)
            {
                StopCoroutine(statusLevelPeriodicCoroutine);
                statusLevelPeriodicCoroutine = null;
            }
            statusLevelPeriodicCoroutine = StartCoroutine(HandlePeriodicSelection());
        }

        private IEnumerator HandlePeriodicSelection()
        {
            while (currentStatusEffectConfig.isPeriodicStatusLevel)
            {
                float duration;

                //if time is random
                if (currentStatusEffectConfig.isDurationRandom)
                {
                    duration = Random.Range(currentStatusEffectConfig.minDuration, currentStatusEffectConfig.maxDuration);
                }
                //if time is preset
                else
                {
                    duration = currentStatusEffectConfig.duration;
                }
                yield return new WaitForSeconds(duration);

                //end previous effects
                StartEndingAllStatusEffects();

                float restartWaitDuration;

                //if time is random
                if (currentStatusEffectConfig.isRestartWaitTimeRandom)
                {
                    restartWaitDuration = Random.Range(currentStatusEffectConfig.minWaitTime, currentStatusEffectConfig.maxWaitTime);
                }
                //if time is preset
                else
                {
                    restartWaitDuration = currentStatusEffectConfig.restartWaitTime;
                }
                yield return new WaitForSeconds(restartWaitDuration);

                //now select the new effect
                SetCurrentStatusEffect();
            }
            statusLevelPeriodicCoroutine = null;
        }

        /// <summary>
        /// Handles selecting the next effect
        /// </summary>
        private void SetCurrentStatusEffect()
        {
            if (currentStatusEffectConfig.statusEffectsData.Count == 0)
            {
                Debug.LogError("StatusEffectSettings.statusEffectsData list is empty.  This must contain at least 1 element.");
                return;
            }

            //select a random effect
            if (currentStatusEffectConfig.chooseRandomStatusEffect)
            {
                //choose a random status effect from the list of preset status effects
                int effectIndex = Random.Range(0, currentStatusEffectConfig.statusEffectsData.Count);

                OnStatusEffectSelected.Invoke(currentStatusEffectConfig.statusEffectsData[effectIndex]);
                return;
            }
            //otherwise run all the status effects in our effects list
            for (int i = 0; i < currentStatusEffectConfig.statusEffectsData.Count; i++)
            {
                OnStatusEffectSelected.Invoke(currentStatusEffectConfig.statusEffectsData[i]);
            }
        }

        private IEnumerator TimeToSelectStatusEffect(StatusEffectSettings.StatusLevelConfig effects)
        {
            yield return new WaitForSeconds(currentTimeToSelectEffect);

            SetCurrentStatusEffect();
            StartPeriodicSelection();

            statusEffectSelectionCoroutine = null;
        }

        /// <summary>
        /// In the event we need to cancel the status effect selection process for any reason
        /// </summary>
        public void CancelStatusEffectSelection()
        {
            if (statusEffectSelectionCoroutine != null)
            {
                StopCoroutine(statusEffectSelectionCoroutine);
                statusEffectSelectionCoroutine = null;
            }
        }

        /// <summary>
        /// In the event we need to cancel the periodic effect selection process for any reason
        /// </summary>
        public void CancelPeriodicEffectSelection()
        {
            if (statusLevelPeriodicCoroutine != null)
            {
                StopCoroutine(statusLevelPeriodicCoroutine);
                statusLevelPeriodicCoroutine = null;
            }
        }

        /// <summary>
        /// Immediately hard stop all status effects
        /// </summary>
        public void EndAllStatusEffects()
        {
            foreach (StatusEffect effect in statusEffects)
            {
                effect.SetState(StatusEffect.StatusEffectState.Ended);
            }
        }

        /// <summary>
        /// Immediately start the ending process to ease out of the status effects
        /// </summary>
        public void StartEndingAllStatusEffects()
        {
            foreach (StatusEffect effect in statusEffects)
            {
                effect.StartEndingStatusEffect();
            }
        }

        /// <summary>
        /// Do any reset logic after the status effect logic has finished
        /// </summary>
        private void StatusEffectEnded()
        {

        }

        /// <summary>
        /// Select a random duration
        /// </summary>
        /// <returns></returns>
        private float SelectStatusEffectTimer()
        {
            return Random.Range(configFields.minTimeToSelectEffect, configFields.maxTimeToSelectEffect);
        }

        private void OnDestroy()
        {
            CancelStatusEffectSelection();

            StatusManager.Instance.OnStatusLevelChanged.RemoveListener(SelectStatusEffect);
            foreach (StatusEffect effect in statusEffects)
            {
                effect.OnStatusEffectEnded.RemoveListener(StatusEffectEnded);
            }
        }
    }
}