/*
 * Description: Basic class handling enemy's animation controller
 */

using UnityEngine;

namespace Gameplay.System.Enemy
{
    public class EnemyAnimationController : MonoBehaviour
    {
        const string PARAM_BOOL_MOVING = "IsMoving";
        const string PARAM_TRIGGER_ATTACK = "Attack";

        private static int paramTriggerAttack;
        private static int paramIsMoving;

        [SerializeField]
        private Animator enemyAnimator;

        private void Awake()
        {
            paramTriggerAttack = Animator.StringToHash(PARAM_TRIGGER_ATTACK);
            paramIsMoving = Animator.StringToHash(PARAM_BOOL_MOVING);
        }

        public void TriggerAttack()
        {
            enemyAnimator.SetTrigger(paramTriggerAttack);
        }

        public void SetIsMoving(bool state)
        {
            enemyAnimator.SetBool(paramIsMoving, state);
        }

        public void ResetTriggers()
        {
            enemyAnimator.ResetTrigger(paramTriggerAttack);
        }
    }
}