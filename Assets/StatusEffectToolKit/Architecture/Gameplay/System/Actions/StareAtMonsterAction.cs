/*
 * Description: Example script where looking at a monster too long can increase the status level
 */

using UnityEngine;
using Service.Framework.StatusSystem;
using System.Collections.Generic;

namespace Gameplay.System.StatusSystem.Actions
{
    public class StareAtMonsterAction : ActionHandler
    {
        [SerializeField]
        private float lookThreshold = 0.6f;
        [SerializeField]
        private float maxLookDistance = 100.0f;

        private bool isLookingAtMonster = false;

        private Transform mainCam;

        private List<GameObject> monsters = new List<GameObject>();
        public List<GameObject> Monsters
        {
            get { return monsters; }
            set { monsters = value; }
        }

        public override void Start()
        {
            base.Start();
            mainCam = Camera.main.transform;
        }

        public void RegisterMonster(GameObject entity)
        {
            if (!monsters.Contains(entity))
            {
                monsters.Add(entity);
            }
        }

        public void UnRegisterMonster(GameObject entity)
        {
            if (monsters.Contains(entity))
            {
                monsters.Remove(entity);
            }
            ResetLookState();
        }

        public override void ActionUpdate(float deltaTime)
        {
            UpdateLookState();
            HandleLookingAtMonster(deltaTime);
        }

        private void UpdateLookState()
        {
            if (monsters.Count == 0)
            {
                return;
            }

            for (int i = 0; i < monsters.Count; i++)
            {
                //check from the monster's point of view, whether or not the player is looking at this monster
                Vector3 toMonster = (monsters[i].transform.position - mainCam.position).normalized;
                float dotPlayer = Vector3.Dot(mainCam.forward, toMonster);

                //if outside a cone of view
                if (dotPlayer < lookThreshold)
                {
                    //reset if we were previously looking at the monster
                    if (isLookingAtMonster)
                    {
                        ResetLookState();
                    }
                    continue;
                }

                //check for obstructions
                if (Physics.Raycast(mainCam.position, toMonster, out RaycastHit hitInfo, maxLookDistance))
                {
                    bool isDetected = hitInfo.transform == monsters[i].transform;

                    //if we are looking at the monster for the first time
                    if (isDetected && !isLookingAtMonster)
                    {
                        isLookingAtMonster = true;
                        return;
                    }
                    //there is a new obstruction
                    else if (!isDetected && isLookingAtMonster)
                    {
                        ResetLookState();
                    }
                }
            }
        }

        private void HandleLookingAtMonster(float deltaTime)
        {
            //do nothing if we aren't looking at the monster
            if (!isLookingAtMonster)
            {
                return;
            }
            //stop tracking status level if we already maxed out
            if (StatusManager.Instance.StatusLevel == StatusManager.Instance.StatusSettings.maxStatusLevel)
            {
                ResetLookState();
                return;
            }
            //we're looking at the monster, so start tracking
            if (!StatusLevelAdjuster.Instance.GetStatusTrackingState())
            {
                StatusLevelAdjuster.Instance.StartTrackingStatus(threshold, true);
            }
        }

        private void ResetLookState()
        {
            if (StatusLevelAdjuster.Instance.GetStatusTrackingState())
            {
                StatusLevelAdjuster.Instance.StopTrackingStatus();
            }
            isLookingAtMonster = false;
        }
    }
}