/*
 * Description: Example script that moves an object to a position and then lowers the status level
 */

using Service.Core;
using Service.Framework.StatusSystem;
using System.Collections;
using UnityEngine;

namespace Gameplay.System.StatusSystem.Actions
{
    public class CompleteObjectiveAction : ActionHandler, IInteractable
    {
        [SerializeField]
        private ActivateObjectTrigger activationTrigger;

        [SerializeField]
        private Transform endPoint;
        [SerializeField]
        private Transform movingMechanism;

        [SerializeField]
        private float moveSpeed = 10f;

        private float distanceThresholdFromEndpoint = 0.01f;

        private bool canInteract;
        private bool objectiveComplete;

        private Coroutine moveToPositionCoroutine;

        public override void Start()
        {
            base.Start();
            activationTrigger.OnEnteredTriggerObject.AddListener(OnMechanismInteractable);
            activationTrigger.OnExitedTriggerObject.AddListener(OnMechanismNotInteractable);
        }

        private void OnMechanismInteractable()
        {
            ReferenceRegistry.Instance.MainUi.SetContextualUiVisible(true);
            canInteract = true;
        }

        private void OnMechanismNotInteractable()
        {
            ReferenceRegistry.Instance.MainUi.SetContextualUiVisible(false);
            canInteract = false;
        }

        public void Interact(GameObject interactor)
        {
            if (!canInteract || objectiveComplete)
            {
                return;
            }
            ActivateMechanism();
        }

        private void ActivateMechanism()
        {
            if (!canInteract || objectiveComplete)
            {
                return;
            }

            //start the coroutine
            if (moveToPositionCoroutine == null)
            {
                moveToPositionCoroutine = StartCoroutine(MoveMechanism());
            }
        }

        private IEnumerator MoveMechanism()
        {
            //move towards the endpoint until reaching a tiny distance from that point
            while (Vector3.Distance(movingMechanism.position, endPoint.position) > distanceThresholdFromEndpoint)
            {
                movingMechanism.transform.position = Vector3.MoveTowards(movingMechanism.position, endPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            StatusLevelAdjuster.Instance.DecreaseStatusLevel(modifyStatusLevelAmount);
            objectiveComplete = true;
        }

        private void OnDestroy()
        {
            activationTrigger.OnEnteredTriggerObject.RemoveListener(OnMechanismInteractable);
            activationTrigger.OnExitedTriggerObject.RemoveListener(OnMechanismNotInteractable);

            moveToPositionCoroutine = null;
        }
    }
}