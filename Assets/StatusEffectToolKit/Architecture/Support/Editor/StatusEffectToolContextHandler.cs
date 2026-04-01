/*
 * Description: Handles the conditional logic for drawing fields
 */
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;

namespace Support.Editor
{
    static class StatusEffectToolContextHandler
    {
        /// <summary>
        /// Reset to default values for the effect selection fields
        /// </summary>
        /// <param name="property"></param>
        public static void SyncManagerConfigDefaultValues(SerializedProperty property)
        {
            if (property == null)
            {
                return;
            }

            SerializedProperty requireWaitTime = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.REQUIRE_TIME);
            if (requireWaitTime == null)
            {
                return;
            }

            if (!requireWaitTime.boolValue)
            {
                SerializedProperty isRandomTime = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_RANDOM_TIME);
                if (isRandomTime != null && isRandomTime.boolValue)
                {
                    isRandomTime.boolValue = StatusEffectsDataFieldRegistry.DEFAULT_IS_WAIT_TIME_RANDOM;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        /// <summary>
        /// Reset to default values for status effect fields
        /// </summary>
        /// <param name="property"></param>
        public static void SyncStatusEffectDefaultValues(SerializedProperty property)
        {
            if (property == null)
            {
                return;
            }

            //set status level defaults
            SerializedProperty isLevelPeriodic = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_STATUS_LEVEL_PERIODIC);
            if (isLevelPeriodic != null && !isLevelPeriodic.boolValue)
            {
                SerializedProperty isLevelDurationRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_LEVEL_DURATION_RANDOM);
                if (isLevelDurationRandom != null && isLevelDurationRandom.boolValue)
                {
                    isLevelDurationRandom.boolValue = StatusEffectsDataFieldRegistry.DEFAULT_IS_DURATION_RANDOM;
                }
                SerializedProperty isLevelWaitTimeRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_LEVEL_WAIT_RANDOM);
                if (isLevelWaitTimeRandom != null && isLevelWaitTimeRandom.boolValue)
                {
                    isLevelWaitTimeRandom.boolValue = StatusEffectsDataFieldRegistry.DEFAULT_IS_WAIT_TIME_RANDOM;
                }
                property.serializedObject.ApplyModifiedProperties();
            }

            //set status effects defaults
            SerializedProperty isPeriodic = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_PERIODIC);
            SerializedProperty requireWaitTime = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.REQUIRE_WAIT_TIME);
            if (isPeriodic != null && !isPeriodic.boolValue)
            {
                SerializedProperty isDurationRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_DURATION_RANDOM);
                if (isDurationRandom != null && isDurationRandom.boolValue)
                {
                    isDurationRandom.boolValue = StatusEffectsDataFieldRegistry.DEFAULT_IS_DURATION_RANDOM;
                }
                SerializedProperty isWaitTimeRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_WAIT_TIME_RANDOM);
                if (isWaitTimeRandom != null && isWaitTimeRandom.boolValue)
                {
                    isWaitTimeRandom.boolValue = StatusEffectsDataFieldRegistry.DEFAULT_IS_WAIT_TIME_RANDOM;
                }
                property.serializedObject.ApplyModifiedProperties();
            }

            if (requireWaitTime != null && !requireWaitTime.boolValue)
            {
                SerializedProperty isStartRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_START_WAIT_RANDOM);
                if (isStartRandom != null && isStartRandom.boolValue)
                {
                    isStartRandom.boolValue = StatusEffectsDataFieldRegistry.DEFAULT_IS_START_WAIT_TIME_RANDOM;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        /// <summary>
        /// Get the fields to draw on the status effect's component
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<SerializedProperty> GetInspectorProperties(SerializedProperty property)
        {
            if (property == null)
            {
                yield break;
            }
            yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.STATUS_EFFECT);
            yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.SEVERITY);
        }

        /// <summary>
        /// Get the status effect's fields on the tool window
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<SerializedProperty> GetStatusEffectVisibleProperties(SerializedProperty property)
        {
            if (property == null)
            {
                yield break;
            }
            yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.STATUS_EFFECT);
            yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.SEVERITY);

            SerializedProperty requireWaitTime = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.REQUIRE_WAIT_TIME);
            yield return requireWaitTime;

            //if setting a restart wait time
            if (requireWaitTime != null && requireWaitTime.boolValue)
            {
                SerializedProperty isRandomWait = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_START_WAIT_RANDOM);
                yield return isRandomWait;

                //only draw min and max when choosing random time, otherwise draw single set value
                if (isRandomWait != null && isRandomWait.boolValue)
                {
                    yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MIN_START_WAIT_TIME);
                    yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MAX_START_WAIT_TIME);
                }
                else
                {
                    yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.START_WAIT_TIME);
                }
            }

            SerializedProperty isPeriodic = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_PERIODIC);
            yield return isPeriodic;

            //don't draw anything else if periodic is false
            if (isPeriodic == null || !isPeriodic.boolValue)
            {
                yield break;
            }

            SerializedProperty isDurationRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_DURATION_RANDOM);
            yield return isDurationRandom;

            //only draw min and max when choosing random time, otherwise draw single set value
            if (isDurationRandom != null && isDurationRandom.boolValue)
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MIN_DURATION);
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MAX_DURATION);
            }
            else
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.DURATION);
            }

            SerializedProperty isWaitRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_WAIT_TIME_RANDOM);
            yield return isWaitRandom;

            //only draw min and max when choosing random time, otherwise draw single set value
            if (isWaitRandom != null && isWaitRandom.boolValue)
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MIN_WAIT_TIME);
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MAX_WAIT_TIME);
            }
            else
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.WAIT_TIME);
            }
        }

        public static IEnumerable<SerializedProperty> GetStatusLevelSettingsVisibleProperties(SerializedProperty property)
        {
            if (property == null)
            {
                yield break;
            }

            //always show choose random
            SerializedProperty chooseRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.CHOOSE_RANDOM_EFFECT);
            if (chooseRandom != null)
            {
                yield return chooseRandom;
            }

            //always show is periodic
            SerializedProperty isPeriodic = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_STATUS_LEVEL_PERIODIC);
            if (isPeriodic != null)
            {
                yield return isPeriodic;
            }

            //if periodic is false, don't draw anything else
            if (isPeriodic == null || !isPeriodic.boolValue)
            {
                yield break;
            }

            SerializedProperty isDurationRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_LEVEL_DURATION_RANDOM);
            yield return isDurationRandom;

            if (isDurationRandom != null && isDurationRandom.boolValue)
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MIN_LEVEL_DURATION);
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MAX_LEVEL_DURATION);
            }
            else
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.LEVEL_DURATION);
            }

            SerializedProperty isWaitRandom = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_LEVEL_WAIT_RANDOM);
            yield return isWaitRandom;

            if (isWaitRandom != null && isWaitRandom.boolValue)
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MIN_LEVEL_WAIT);
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MAX_LEVEL_WAIT);
            }
            else
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.LEVEL_WAIT);
            }
        }

        public static IEnumerable<SerializedProperty> GetEffectSelectionVisibleProperties(SerializedProperty property)
        {
            if (property == null)
            {
                yield break;
            }

            SerializedProperty requireSelectionTime = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.REQUIRE_TIME);

            if (requireSelectionTime != null)
            {
                yield return requireSelectionTime;
            }

            //if require selection time is false, don't draw anything else
            if (requireSelectionTime == null || !requireSelectionTime.boolValue)
            {
                yield break;
            }

            SerializedProperty isRandomTime = property.FindPropertyRelative(StatusEffectsDataFieldRegistry.IS_RANDOM_TIME);
            yield return isRandomTime;

            if (isRandomTime != null && isRandomTime.boolValue)
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MIN_TIME);
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.MAX_TIME);
            }
            else
            {
                yield return property.FindPropertyRelative(StatusEffectsDataFieldRegistry.TIME_TO_SELECT);
            }
        }
    }
}
#endif