/*
 * Description: Handles turning on or off objects 
 */

using Service.Framework.StatusSystem;
using UnityEngine;

namespace Gameplay.System.StatusSystem
{
    public class Hallucinations_StatusEffect : StatusEffect
    {
        [SerializeField]
        private GameObject[] objectRefs;

        [SerializeField]
        private float playerRadius = 8f;

        public override void HandleRunStart()
        {
            for (int i = 0; i < objectRefs.Length; i++)
            {
                objectRefs[i].SetActive(true);
                objectRefs[i].transform.position = GetPositionAroundPlayer();
            }
            SetState(StatusEffectState.Running);
        }

        /// <summary>
        /// Get a position within a radius from the player
        /// </summary>
        /// <returns></returns>
        private Vector3 GetPositionAroundPlayer()
        {
            return ReferenceRegistry.Instance.Player.transform.position + new Vector3(
                (playerRadius * Random.insideUnitCircle).x, 0, (playerRadius * Random.insideUnitCircle).y);
        }

        public override void StatusEffectEnding(float deltaTime)
        {
            for (int i = 0; i < objectRefs.Length; i++)
            {
                objectRefs[i].SetActive(false);
            }
            SetState(StatusEffectState.Ended);
        }

        public override void StatusEffectEnded()
        {
            base.StatusEffectEnded();
            for (int i = 0; i < objectRefs.Length; i++)
            {
                objectRefs[i].SetActive(false);
            }
        }
    }
}