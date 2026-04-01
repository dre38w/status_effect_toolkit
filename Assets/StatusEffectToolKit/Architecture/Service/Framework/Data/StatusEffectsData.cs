
/*
Description: Holds some example of status effects
This data is used so systems can choose what effect or severity to use
*/

using System;

namespace Service.Framework.StatusSystem
{
    [Serializable]
    public class StatusEffectsData
    {
        public StatusEffects statusEffect;
        //can be used to modify intensity of the effect
        public Severity severity;

        public bool requireWaitTimeToStart;
        public bool isStartWaitTimeRandom;

        public float minWaitTimeToStart;
        public float maxWaitTimeToStart;
        public float waitTimeToStart;

        public bool isPeriodicEffect;

        public bool isDurationRandom;

        public float minDuration;
        public float maxDuration;
        public float duration;

        public bool isRestartWaitTimeRandom;

        public float minWaitTime;
        public float maxWaitTime;
        public float restartWaitTime;
    }

    /// <summary>
    /// List of available effects
    /// </summary>
    public enum StatusEffects
    {
        Stable,
        Freeze,
        TunnelVision,
        Hallucinations,
        Death,
    }

    /// <summary>
    /// List of available severity levels
    /// </summary>
    public enum Severity
    {
        Minor,
        Moderate,
        Major,
        Critical,
    }
}