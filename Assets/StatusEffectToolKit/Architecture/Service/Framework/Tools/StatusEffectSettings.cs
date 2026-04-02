/*
 * Description: The SO that holds the status effect data that's read by the game 
 */

using Service.Framework.StatusSystem;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Service.Framework.Tools
{
    public class StatusEffectSettings : ScriptableObject
    {
        public int maxStatusLevel;

        public List<StatusLevelConfig> statusLevels = new List<StatusLevelConfig>();

        public StatusEffectManager.ConfigFields statusEffectManagerConfig = new StatusEffectManager.ConfigFields();

        /// <summary>
        /// The data associated with the overall Status Level
        /// </summary>
        [Serializable]
        public class StatusLevelConfig
        {
            public bool chooseRandomStatusEffect;
            public bool isPeriodicStatusLevel;

            public bool isDurationRandom;

            public float minDuration;
            public float maxDuration;
            public float duration;

            public bool isRestartWaitTimeRandom;

            public float minWaitTime;
            public float maxWaitTime;
            public float restartWaitTime;

            public List<StatusEffectsData> statusEffectsData = new List<StatusEffectsData>();
        }
    }
}