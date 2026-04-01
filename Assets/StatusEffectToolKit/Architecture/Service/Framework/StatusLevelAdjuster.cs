/*
 * Description: Global helper class that handles tracking the status level.  
 * Allows systems to effect the status level in more dynamic ways such as more efficiently interrupting another action, etc.
 */

using UnityEngine;
using UnityEngine.Events;

namespace Service.Framework.StatusSystem
{
    public class StatusLevelAdjuster : MonoBehaviour
    {

        public static StatusLevelAdjuster Instance;

        public class OnStatusLevelUpdateEvent : UnityEvent<int>
        {

        }
        public OnStatusLevelUpdateEvent OnStatusLevelUpdate = new OnStatusLevelUpdateEvent();

        public bool IsTrackingStatusLevel { get; private set; }

        [SerializeField]
        private float baseThreshold = 5f;
        private float threshold;
        private float currentThreshold;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            currentThreshold = baseThreshold;
        }

        private void Update()
        {
            HandleTrackingStatusLevel();
        }

        private void HandleTrackingStatusLevel()
        {
            //stop tracking status level if we already maxed out
            if (StatusManager.Instance.StatusLevel == StatusManager.Instance.StatusSettings.maxStatusLevel || !IsTrackingStatusLevel)
            {
                return;
            }

            if (currentThreshold > 0)
            {
                currentThreshold -= Time.deltaTime;
            }

            if (currentThreshold <= 0)
            {
                //reset the data 
                currentThreshold = threshold;
                IncreaseStatusLevel();
            }
        }

        public bool GetStatusTrackingState()
        {
            return IsTrackingStatusLevel;
        }

        public void DecreaseStatusLevel(int value = 1)
        {
            OnStatusLevelUpdate.Invoke(-value);
        }

        public void IncreaseStatusLevel(int value = 1)
        {
            OnStatusLevelUpdate.Invoke(value);
        }

        public void StartTrackingStatus(float newThreshold, bool overrideThreshold = false)
        {
            //if overriding with a custom threshold
            if (overrideThreshold)
            {
                threshold = newThreshold;
            }
            //otherwise, use default
            else
            {
                threshold = baseThreshold;
            }
            IsTrackingStatusLevel = true;
        }

        public void StopTrackingStatus()
        {
            IsTrackingStatusLevel = false;
        }

        public void ResetStatusTracker()
        {
            threshold = baseThreshold;
        }
    }
}