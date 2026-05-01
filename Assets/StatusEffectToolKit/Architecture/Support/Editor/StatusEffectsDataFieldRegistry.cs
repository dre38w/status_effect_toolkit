/*
 * Description: Registers the fields that we want to visually modify in the inspector
 */
using Service.Framework.StatusSystem;
using Service.Framework.Tools;

namespace Support.Editor
{
    static class StatusEffectsDataFieldRegistry
    {
        /// <summary>
        /// status effect selection settings fields
        /// </summary>
        public const string REQUIRE_TIME = nameof(StatusEffectManager.ConfigFields.requireWaitTimeBetweenEffects);
        public const string IS_RANDOM_TIME = nameof(StatusEffectManager.ConfigFields.isRandomTimeToSelectEffects);
        public const string TIME_TO_SELECT = nameof(StatusEffectManager.ConfigFields.timeToSelectEffect);
        public const string MAX_TIME = nameof(StatusEffectManager.ConfigFields.maxTimeToSelectEffect);
        public const string MIN_TIME = nameof(StatusEffectManager.ConfigFields.minTimeToSelectEffect);


        /// <summary>
        /// Status level fields
        /// </summary>

        //periodic status level fields
        public const string IS_STATUS_LEVEL_PERIODIC = nameof(StatusEffectSettings.StatusLevelConfig.isPeriodicStatusLevel);
        public const string IS_LEVEL_DURATION_RANDOM = nameof(StatusEffectSettings.StatusLevelConfig.isDurationRandom);
        public const string MIN_LEVEL_DURATION = nameof(StatusEffectSettings.StatusLevelConfig.minDuration);
        public const string MAX_LEVEL_DURATION = nameof(StatusEffectSettings.StatusLevelConfig.maxDuration);
        public const string LEVEL_DURATION = nameof(StatusEffectSettings.StatusLevelConfig.duration);

        //restart wait time fields
        public const string IS_LEVEL_WAIT_RANDOM = nameof(StatusEffectSettings.StatusLevelConfig.isRestartWaitTimeRandom);
        public const string MIN_LEVEL_WAIT = nameof(StatusEffectSettings.StatusLevelConfig.minWaitTime);
        public const string MAX_LEVEL_WAIT = nameof(StatusEffectSettings.StatusLevelConfig.maxWaitTime);
        public const string LEVEL_WAIT = nameof(StatusEffectSettings.StatusLevelConfig.restartWaitTime);

        public const string CHOOSE_RANDOM_EFFECT = nameof(StatusEffectSettings.StatusLevelConfig.chooseRandomStatusEffect);


        /// <summary>
        /// Status effect fields
        /// </summary>

        public const string STATUS_EFFECT = nameof(StatusEffectsData.statusEffect);
        public const string SEVERITY = nameof(StatusEffectsData.severity);

        //wait time to start fields
        public const string REQUIRE_WAIT_TIME = nameof(StatusEffectsData.requireWaitTimeToStart);
        public const string IS_START_WAIT_RANDOM = nameof(StatusEffectsData.isStartWaitTimeRandom);
        public const string MIN_START_WAIT_TIME = nameof(StatusEffectsData.minWaitTimeToStart);
        public const string MAX_START_WAIT_TIME = nameof(StatusEffectsData.maxWaitTimeToStart);
        public const string START_WAIT_TIME = nameof(StatusEffectsData.waitTimeToStart);

        //effect periodic fields
        public const string IS_PERIODIC = nameof(StatusEffectsData.isPeriodicEffect);
        public const string IS_DURATION_RANDOM = nameof(StatusEffectsData.isDurationRandom);
        public const string MIN_DURATION = nameof(StatusEffectsData.minDuration);
        public const string MAX_DURATION = nameof(StatusEffectsData.maxDuration);
        public const string DURATION = nameof(StatusEffectsData.duration);

        //restart wait time fields
        public const string IS_WAIT_TIME_RANDOM = nameof(StatusEffectsData.isRestartWaitTimeRandom);
        public const string MIN_WAIT_TIME = nameof(StatusEffectsData.minWaitTime);
        public const string MAX_WAIT_TIME = nameof(StatusEffectsData.maxWaitTime);
        public const string WAIT_TIME = nameof(StatusEffectsData.restartWaitTime);

        //default value fields
        public const bool DEFAULT_IS_DURATION_RANDOM = false;
        public const bool DEFAULT_IS_WAIT_TIME_RANDOM = false;
        public const bool DEFAULT_IS_START_WAIT_TIME_RANDOM = false;
    }
}