/*
 * Description: Handles logic for turning a light on and off.  
 * Handles being in the dark/light when within range of light
 */

using Service.Framework.StatusSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.System.StatusSystem.Actions
{
    public class HandleDarknessAction : ActionHandler
    {
        private GameObject player;

        private List<Light> lights = new List<Light>();

        public override void Start()
        {
            base.Start();
            player = ReferenceRegistry.Instance.Player.gameObject;
        }

        public void RegisterLight(Light entity)
        {
            if (!lights.Contains(entity))
            {
                lights.Add(entity);
            }
        }

        public void UnRegisterLight(Light entity)
        {
            if (lights.Contains(entity))
            {
                lights.Remove(entity);
            }
        }

        public override void ActionUpdate(float deltaTime)
        {
            HandleDistanceChecking(deltaTime);
        }

        private void HandleDistanceChecking(float deltaTime)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                //see if the player is in range of any of the lights
                float distance = Vector3.Distance(player.transform.position, lights[i].transform.position);

                //restart the loop until we find one
                if (distance > lights[i].range)
                {
                    continue;
                }
                //if the intensity is 0 then the light is off and we are in the dark
                if (lights[i].intensity <= 0)
                {
                    if (!StatusLevelAdjuster.Instance.GetStatusTrackingState())
                    {
                        StatusLevelAdjuster.Instance.StartTrackingStatus(threshold, true);
                    }
                }
            }
        }
    }
}