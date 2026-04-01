/*
Description: Handles main logic for the status effect system.
Updates status level, routes corosponding status effects, etc.
*/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Service.Framework.Tools;

namespace Service.Framework.StatusSystem
{
    public class StatusManager : MonoBehaviour
    {
        public static StatusManager Instance;

        //event that will pass the status level and associated status effects
        public class OnStatusLevelChangedEvent : UnityEvent<int, StatusEffectSettings.StatusLevelConfig> { }

        [HideInInspector]
        public OnStatusLevelChangedEvent OnStatusLevelChanged = new OnStatusLevelChangedEvent();

        private StatusEffectSettings.StatusLevelConfig currentStatusLevelConfig;
        public StatusEffectSettings.StatusLevelConfig CurrentStatusLevelConfig
        {
            get { return currentStatusLevelConfig; }
            set { currentStatusLevelConfig = value; }
        }
        private StatusEffectSettings statusSettings;
        public StatusEffectSettings StatusSettings
        {
            get { return statusSettings; }
            set { statusSettings = value; }
        }

        private int statusLevel;
        public int StatusLevel
        {
            get { return statusLevel; }
            set { statusLevel = value; }
        }

        private List<ActionHandler> actionHandlers = new List<ActionHandler>();
        public List<ActionHandler> ActionHandlers
        {
            get { return actionHandlers; }
            set { actionHandlers = value; }
        }

        private bool isActionHandlerUpdating = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            statusSettings = StatusSettingsUtility.StatusEffectSettings;

            ValidateData();
        }

        /// <summary>
        /// Validate any required data
        /// </summary>
        private void ValidateData()
        {
            if (statusSettings.statusLevels.Count != statusSettings.maxStatusLevel)
            {
                Debug.LogError("Status Level count is not equal to the max status level." +
                    "Please open the Status Level Toolkit and make sure the status level count is equal to the max status level.");
            }
        }

        private void Start()
        {
            statusLevel = 0;

            if (statusSettings == null)
            {
                return;
            }

            if (statusSettings.statusLevels == null || statusSettings.statusLevels.Count == 0)
            {
                Debug.LogError("StatusEffectSettings.statusEffectsData is empty.  Index 0 must exist.  Please populate it in the Status Effects Tool.");
            }

            //initialize the config
            currentStatusLevelConfig = statusSettings.statusLevels[0];

            StatusLevelAdjuster.Instance.OnStatusLevelUpdate.AddListener(OnStatusLevelUpdate);
        }

        /// <summary>
        /// Add the status actions
        /// </summary>
        /// <param name="action"></param>
        public static void AddActionHandlers(ActionHandler action)
        {
            if (!Instance.actionHandlers.Contains(action))
            {
                Instance.actionHandlers.Add(action);
            }
        }

        public static void RemoveActionHandlers(ActionHandler action)
        {
            if (Instance.actionHandlers.Contains(action))
            {
                Instance.actionHandlers.Remove(action);
            }
        }

        public void ClearActionHandlers()
        {
            actionHandlers.Clear();
        }

        /// <summary>
        /// In the event we need to remove any null handlers
        /// Useful when accessing cross scene objects or using dynamic register/unregister systems
        /// </summary>
        public void CleanUpActionHandlers()
        {
            for (int i = 0; i < actionHandlers.Count; i++)
            {
                if (actionHandlers[i] == null)
                {
                    actionHandlers.RemoveAt(i);
                }
            }
        }

        private void Update()
        {
            UpdateActionHandlers();
        }

        private void UpdateActionHandlers()
        {
            if (!isActionHandlerUpdating)
            {
                return;
            }
            if (actionHandlers.Count > 0)
            {
                for (int i = 0; i < actionHandlers.Count; i++)
                {
                    actionHandlers[i].ActionUpdate(Time.deltaTime);
                }
            }
        }

        public void SetActionHandlersUpdateState(bool state)
        {
            isActionHandlerUpdating = state;
        }

        /// <summary>
        /// Called via messages and external systems when a factor changes the status level
        /// </summary>
        /// <param name="value">Pass positive value to increase and a negative value to decrease</param>
        public void OnStatusLevelUpdate(int value)
        {
            statusLevel += value;
            statusLevel = Mathf.Clamp(statusLevel, 0, statusSettings.statusLevels.Count - 1);

            currentStatusLevelConfig = statusSettings.statusLevels[statusLevel];

            //invoke the event passing the updated status effects
            //this data is the core of the status system and is what allows the execution of the status effects
            OnStatusLevelChanged.Invoke(statusLevel, currentStatusLevelConfig);
        }

        public void SetStatusLevel(int value)
        {
            statusLevel = value;
            currentStatusLevelConfig = statusSettings.statusLevels[statusLevel];
            OnStatusLevelChanged.Invoke(statusLevel, currentStatusLevelConfig);
        }

        /// <summary>
        /// In the event other managers need to control the max level
        /// </summary>
        /// <param name="value"></param>
        public void SetMaxStatusLevel(int value)
        {
            statusSettings.maxStatusLevel = value;
            ValidateData();
        }

        private void OnDestroy()
        {
            StatusLevelAdjuster.Instance.OnStatusLevelUpdate.RemoveListener(OnStatusLevelUpdate);
        }
    }
}