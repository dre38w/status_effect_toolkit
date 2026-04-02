
/*
Description: Base class for all status effects
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Service.Framework.StatusSystem
{
    public class StatusEffect : MonoBehaviour
    {
        //the state in which the status effect is currently in.
        public enum StatusEffectState
        {
            None,
            Started,    //state used to do initialization
            RunStart,   //state used to do intro logic, one shot behaviors, etc.
            Running,    //used while status effect is currently executing.
            Ending,     //used when executing logic for ending the status effect such as reversing an effect, transitioning into a new effect, etc.
            Ended,      //used to do any final clean up logic
            Pending,    //used when the status effect is paused, is waiting to start, etc.
        }
        [HideInInspector]
        public StatusEffectState State;

        public class OnStatusEffectStartedEvent : UnityEvent<StatusEffect> { }
        public OnStatusEffectStartedEvent OnStatusEffectStarted = new OnStatusEffectStartedEvent();

        [HideInInspector]
        public UnityEvent OnStatusEffectEnded = new UnityEvent();
        
        [Tooltip("Set the status effect and severity to associate with this class and its purpose.")]
        public StatusEffectsData statusEffectData;

        //The current status effect and its associated data
        private StatusEffectsData currentStatusEffectData;

        private float effectDuration;
        private float effectWaitTime;

        private Coroutine effectRunCoroutine;
        private Coroutine effectWaitCoroutine;

        //is the effect currently running?
        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        private void Start()
        {
            StatusEffectManager.Instance.OnStatusEffectSelected.AddListener(StatusEffectSelected);
            RegisterStatusEffect();
        }

        public virtual void RegisterStatusEffect()
        {
            StatusEffectManager.AddStatusEffect(this);
        }

        public virtual void UnRegisterStatusEffect()
        {
            StatusEffectManager.RemoveStatusEffect(this);
        }

        /// <summary>
        /// Do Update logic based on State
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void StatusEffectUpdate(float deltaTime)
        {
            switch (State)
            {
                case StatusEffectState.Running:
                    HandleStatusEffectRunning(deltaTime);
                    break;
                case StatusEffectState.Ending:
                    StatusEffectEnding(deltaTime);
                    break;
                case StatusEffectState.Pending:
                    StatusEffectPending(deltaTime);
                    break;
            }

        }

        /// <summary>
        /// Will be used to select a status effect in order to do logic specific to that effect
        /// </summary>
        /// <param name="data"></param>
        public void StatusEffectSelected(StatusEffectsData data)
        {
            if (data == null)
            {
                return;
            }
            //check if the data passed matches this status effect's data
            if (statusEffectData.statusEffect == data.statusEffect && statusEffectData.severity == data.severity)
            {
                currentStatusEffectData = data;
                SetState(StatusEffectState.Started);
            }
        }

        /// <summary>
        /// Handle initialization logic
        /// </summary>
        /// <param name="data"></param>
        public virtual void HandleStatusEffectStart(StatusEffectsData data)
        {
            //if requiring a wait time before starting the effect
            if (data.requireWaitTimeToStart && !isRunning)
            {
                if (data.isStartWaitTimeRandom)
                {
                    effectWaitTime = Random.Range(data.minWaitTimeToStart, data.maxWaitTimeToStart);
                }
                else
                {
                    effectWaitTime = data.waitTimeToStart;
                }
                if (effectWaitCoroutine == null)
                {
                    effectWaitCoroutine = StartCoroutine(HandleStartWaitTime());
                    return;
                }
            }

            OnStatusEffectStarted.Invoke(this);
            isRunning = true;
            StopWaitCoroutine();

            //if periodic effect, choose the duration in which to wait between playing it
            if (data.isPeriodicEffect)
            {
                if (data.isDurationRandom)
                {
                    effectDuration = Random.Range(data.minDuration, data.maxDuration);
                }
                else
                {
                    effectDuration = data.duration;
                }


                if (effectRunCoroutine == null)
                {
                    effectRunCoroutine = StartCoroutine(HandlePeriodicDuration());
                }
            }
            SetState(StatusEffectState.RunStart);
        }

        /// <summary>
        /// Used to do logic after initialization and right before running.
        /// Useful when handling pending state
        /// </summary>
        public virtual void HandleRunStart()
        {

        }

        /// <summary>
        /// Used while the status effect is running
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void HandleStatusEffectRunning(float deltaTime)
        {

        }

        /// <summary>
        /// Used when doing status effect end logic
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void StatusEffectEnding(float deltaTime)
        {
            
        }

        /// <summary>
        /// Used when you want to pause the status effect
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void StatusEffectPending(float deltaTime)
        {

        }

        /// <summary>
        /// Used when you want to start an Update based ending sequence
        /// </summary>
        public void StartEndingStatusEffect()
        {
            isRunning = false;
            SetState(StatusEffectState.Ending);
        }

        /// <summary>
        /// Used to immediately end the status effect
        /// </summary>
        public virtual void EndStatusEffect()
        {
            isRunning = false;
            SetState(StatusEffectState.Ended);
        }

        /// <summary>
        /// Do logic after the effect has ended
        /// </summary>
        public virtual void StatusEffectEnded()
        {
            OnStatusEffectEnded.Invoke();

            if (isRunning)
            {
                StopRunCoroutine();

                //if this is a periodic status effect, occassionally restart it
                if (currentStatusEffectData.isPeriodicEffect)
                {
                    if (currentStatusEffectData.isRestartWaitTimeRandom)
                    {
                        effectWaitTime = Random.Range(currentStatusEffectData.minWaitTime, currentStatusEffectData.maxWaitTime);
                    }
                    else
                    {
                        effectWaitTime = currentStatusEffectData.restartWaitTime;
                    }
                    if (effectWaitCoroutine == null)
                    {
                        effectWaitCoroutine = StartCoroutine(HandlePeriodicWaitTime());
                    }
                }
            }
            else
            {
                StopRunCoroutine();
                StopWaitCoroutine();
            }
        }

        /// <summary>
        /// Do State transition logic
        /// </summary>
        /// <param name="state"></param>
        public virtual void SetState(StatusEffectState state)
        {
            if (State == state)
            {
                return;
            }
            State = state;

            switch (State)
            {
                case StatusEffectState.Started:
                    HandleStatusEffectStart(currentStatusEffectData);
                    break;
                case StatusEffectState.RunStart:
                    HandleRunStart();
                    break;
                case StatusEffectState.Ended:
                    StatusEffectEnded();
                    break;
            }
        }

        #region Timing Coroutines
        private IEnumerator HandlePeriodicDuration()
        {
            yield return new WaitForSeconds(effectDuration);
            SetState(StatusEffectState.Ending);
        }

        private IEnumerator HandlePeriodicWaitTime()
        {
            yield return new WaitForSeconds(effectWaitTime);
            HandleStatusEffectStart(currentStatusEffectData);
        }

        private IEnumerator HandleStartWaitTime()
        {
            yield return new WaitForSeconds(effectWaitTime);
            OnStatusEffectStarted.Invoke(this);
            isRunning = true;
            HandleStatusEffectStart(currentStatusEffectData);
        }
        #endregion

        /// <summary>
        /// Helper methods to stop coroutines
        /// </summary>
        
        public void StopRunCoroutine()
        {
            if (effectRunCoroutine != null)
            {
                StopCoroutine(effectRunCoroutine);
                effectRunCoroutine = null;
            }
        }

        public void StopWaitCoroutine()
        {
            if (effectWaitCoroutine != null)
            {
                StopCoroutine(effectWaitCoroutine);
                effectWaitCoroutine = null;
            }
        }

        public virtual void OnDestroy()
        {
            StatusEffectManager.Instance.OnStatusEffectSelected.RemoveListener(StatusEffectSelected);

        }
    }
}