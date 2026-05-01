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

        /// <summary>
        /// The config data for the on the overall Status Level including the Status Effect data
        /// </summary>
        private StatusEffectSettings.StatusLevelConfig currentStatusLevelConfig;
        public StatusEffectSettings.StatusLevelConfig CurrentStatusLevelConfig
        {
            get { return currentStatusLevelConfig; }
            set { currentStatusLevelConfig = value; }
        }
        /// <summary>
        /// The ScriptableObject that holds the data that is read by the game
        /// </summary>
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

        /// <summary>
        /// The actions that influence increasing or decreasing the Status Level
        /// </summary>
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
            //load in the data
            statusSettings = StatusSettingsUtility.StatusEffectSettings;
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

        /// <summary>
        /// Update all the action handlers through the manager
        /// </summary>
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

        /// <summary>
        /// Used to set the update state for the action handlers.
        /// This is useful for optimization.  Stopping unnecessary Update calls and restarting them when needed
        /// </summary>
        /// <param name="state"></param>
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

        /// <summary>
        /// Used to set the status level to a specific level.
        /// Useful if a mechanic causes a specific effect to run
        /// </summary>
        /// <param name="value"></param>
        public void SetStatusLevel(int value)
        {
            statusLevel = value;
            currentStatusLevelConfig = statusSettings.statusLevels[statusLevel];
            OnStatusLevelChanged.Invoke(statusLevel, currentStatusLevelConfig);
        }

        private void OnDestroy()
        {
            StatusLevelAdjuster.Instance.OnStatusLevelUpdate.RemoveListener(OnStatusLevelUpdate);
        }
    }
}