
/*
Description: Base class for all action handlers
*/

using UnityEngine;
using System;

namespace Service.Framework.StatusSystem
{
    [Serializable]
    public class ActionHandler : MonoBehaviour
    {
        [Tooltip("The amount of levels to increase/decrease by when changing the status level.")]
        public int modifyStatusLevelAmount = 1;
        [Tooltip("For actions requiring a timer, this is the time it takes for the status level to change in seconds.")]
        public float threshold;

        public virtual void Start()
        {
            StatusManager.AddActionHandlers(this);
        }

        public virtual void ActionUpdate(float deltaTime)
        {

        }

        private void OnDisable()
        {
            StatusManager.RemoveActionHandlers(this);
        }
    }
}