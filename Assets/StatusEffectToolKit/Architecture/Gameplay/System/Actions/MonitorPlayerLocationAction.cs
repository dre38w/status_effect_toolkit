
/*
Description: An example script on something that can effect the sanity level.
             Place this script on trigger boxes placed in desired locations.
*/

using UnityEngine;
using Service.Framework.StatusSystem;
using Service.Framework;

namespace Gameplay.System.StatusSystem.Actions
{
    public class MonitorPlayerLocationAction : ActionHandler
    { 
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagData.PLAYER_TAG))
            {
                if (!StatusLevelAdjuster.Instance.GetStatusTrackingState())
                {
                    StatusLevelAdjuster.Instance.StartTrackingStatus(threshold, true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagData.PLAYER_TAG))
            {
                if (StatusLevelAdjuster.Instance.GetStatusTrackingState())
                {
                    StatusLevelAdjuster.Instance.StopTrackingStatus();
                }
            }
        }
    }
}