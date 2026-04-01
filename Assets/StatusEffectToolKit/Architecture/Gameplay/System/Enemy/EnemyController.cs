/*
 * Description: Enemy class that patrols and chases/attacks player
 */
using Gameplay.System.StatusSystem.Actions;
using Service.Framework;
using Service.Framework.Health;
using Service.Framework.StatusSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Gameplay.System.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        public enum EnemyState
        {
            Patrol,
            Attack,
            Search,
        }
        [HideInInspector]
        public EnemyState State;

        [HideInInspector]
        public UnityEvent OnAttackPlayer = new UnityEvent();

        [SerializeField]
        private float attackDamage = 1f;

        [SerializeField]
        private float patrolRadius = 10f;
        [SerializeField]
        private float alertDistance = 10f;
        [SerializeField]
        private float minAttackDistance = 0.5f;

        [SerializeField]
        private float fieldOfVisionConeAngle = 45f;

        [SerializeField]
        private float waitTimeBetweenWaypoints = 3f;
        [SerializeField]
        private float waitTimeBetweenAttacks = 2f;

        [SerializeField]
        private Transform lineOfSight;
        
        private StareAtMonsterAction stareAtMonsterAction;

        private NavMeshAgent agent;

        private GameObject player;

        private WaitForSeconds waitToSelectPositionEnumerator;
        private WaitForSeconds waitToAttackEnumerator;

        private Coroutine waitToSelectPositionCoroutine;
        private Coroutine searchPlayerCoroutine;
        private Coroutine attackPlayerCoroutine;

        private Vector3 destination;

        private HealthHandler playerHealthHandler;
        private EnemyAnimationController enemyAnimController;

        [SerializeField]
        private bool debugMode = false;

        private void Start()
        {
            player = ReferenceRegistry.Instance.Player.gameObject;
            agent = GetComponent<NavMeshAgent>();
            enemyAnimController = GetComponent<EnemyAnimationController>();

            waitToSelectPositionEnumerator = new WaitForSeconds(waitTimeBetweenWaypoints);
            waitToAttackEnumerator = new WaitForSeconds(waitTimeBetweenAttacks);

            StartCoroutine(RegisterMonster());
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            yield return null;

            playerHealthHandler = player.GetComponent<HealthHandler>();

            if (playerHealthHandler == null)
            {
                Debug.LogError($"Player Health Handler component was not found on the player.  Add the component to the player object.");
            }
        }

        /// <summary>
        /// Registers this monster to the manager
        /// </summary>
        /// <returns></returns>
        private IEnumerator RegisterMonster()
        {
            //wait a single frame to allow the singleton managers to initialize
            yield return null;
            //find the action that will manage this registered monster
            stareAtMonsterAction = (StareAtMonsterAction)StatusManager.Instance.ActionHandlers.Find(a => a is StareAtMonsterAction);
            stareAtMonsterAction.RegisterMonster(gameObject);
        }

        private void OnEnable()
        {
            //re-register monster
            if (stareAtMonsterAction != null)
            {
                stareAtMonsterAction.RegisterMonster(gameObject);
            }
        }

        private void Update()
        {
            //patrol so long as player is not in range
            if (Vector3.Distance(transform.position, player.transform.position) > alertDistance)
            {
                if (searchPlayerCoroutine != null)
                {
                    StopCoroutine(searchPlayerCoroutine);
                    searchPlayerCoroutine = null;
                }
                SetState(EnemyState.Patrol);
                Patrol();
                return;
            }
            //player is close enough to attack
            if (IsPlayerInAttackRange())
            {                    
                if (searchPlayerCoroutine != null)
                {
                    StopCoroutine(searchPlayerCoroutine);
                    searchPlayerCoroutine = null;
                }
                SetState(EnemyState.Attack);
                agent.isStopped = false;
                enemyAnimController.SetIsMoving(true);
                ChasePlayer();
            }
            //player isn't attackable, but is searchable
            else
            {
                SetState(EnemyState.Search);
                agent.isStopped = true;
                enemyAnimController.SetIsMoving(false);
                agent.ResetPath();

                if (searchPlayerCoroutine == null)
                {
                    searchPlayerCoroutine = StartCoroutine(WaitAndSearch());
                }
            }    
        }

        /// <summary>
        /// Check if player is in view and close enough to attack
        /// </summary>
        /// <returns></returns>
        private bool IsPlayerInAttackRange()
        {
            Vector3 origin = lineOfSight.position;
            //get the vector to the player
            Vector3 toPlayer = player.transform.position - origin;

            //get the distance
            float distanceToPlayer = toPlayer.magnitude;

            //player is too far away
            if (distanceToPlayer > alertDistance)
            {
                return false;
            }
            //stop patrolling since player is now within search range
            StopPatrol();

            //get the direction to the player
            Vector3 directionToPlayer = toPlayer / distanceToPlayer;
            //calculate the angle of the vision cone
            float fieldOfViewAngle = Vector3.Angle(lineOfSight.forward, directionToPlayer);

            //player not in vision cone
            if (fieldOfViewAngle > fieldOfVisionConeAngle)
            {
                return false;
            }
            //now that player is close enough and within the vision cone,
            //check if we can see the player without obstructions and return true
            if (Physics.Raycast(origin, directionToPlayer, out RaycastHit hitInfo, distanceToPlayer))
            {
                return hitInfo.transform.CompareTag(TagData.PLAYER_TAG);
            }
            return false;
        }

        /// <summary>
        /// Perform some "search" behaviors
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitAndSearch()
        {
            while (State == EnemyState.Search)
            {
                transform.Rotate(0, 60 * Time.deltaTime, 0);
                yield return null;
            }
            searchPlayerCoroutine = null;
        }

        private void ChasePlayer()
        {
            //move towards the player
            agent.SetDestination(player.transform.position);

            //when within a range, stop chasing the player and attack
            if (Vector3.Distance(transform.position, player.transform.position) <= minAttackDistance)
            {
                if (attackPlayerCoroutine == null)
                {
                    attackPlayerCoroutine = StartCoroutine(AttackPlayer());
                }
                agent.isStopped = true;
                enemyAnimController.SetIsMoving(false);
            }
            else
            {
                agent.isStopped = false;
                enemyAnimController.SetIsMoving(true);

            }
        }

        private IEnumerator AttackPlayer()
        {
            yield return waitToAttackEnumerator;

            OnAttackPlayer.Invoke();
            enemyAnimController.TriggerAttack();
            playerHealthHandler.AdjustHealth(-attackDamage);

            attackPlayerCoroutine = null;
        }

        private void Patrol()
        {
            //choose a random position to move to
            if (waitToSelectPositionCoroutine == null)
            {
                waitToSelectPositionCoroutine = StartCoroutine(WaitToSelectPosition());
            }
            agent.isStopped = false;
            enemyAnimController.SetIsMoving(true);

            agent.SetDestination(destination);

            //close enough to the waypoint
            if (agent.remainingDistance < 0.001f)
            {
                enemyAnimController.SetIsMoving(false);

            }
        }

        public void StopPatrol()
        {
            agent.isStopped = true;
            enemyAnimController.SetIsMoving(false);

            agent.ResetPath();
            if (waitToSelectPositionCoroutine != null)
            {
                StopCoroutine(waitToSelectPositionCoroutine);
                waitToSelectPositionCoroutine = null;
            }
        }

        /// <summary>
        /// Periodically choose a random position to move to
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToSelectPosition()
        {
            yield return waitToSelectPositionEnumerator;
            destination = RandomPositionInRadius();
            waitToSelectPositionCoroutine = null;
        }

        /// <summary>
        /// Choose a random position within a radius around the enemy
        /// </summary>
        /// <returns></returns>
        private Vector3 RandomPositionInRadius()
        {
            return transform.position + new Vector3((patrolRadius * Random.insideUnitCircle).x, 0, (patrolRadius * Random.insideUnitCircle).y);
        }

        public void SetState(EnemyState state)
        {
            if (state == State)
            {
                return;
            }
            State = state;
        }

        private void OnDisable()
        {
            stareAtMonsterAction.UnRegisterMonster(gameObject);
        }

        private void OnDestroy()
        {
            stareAtMonsterAction.UnRegisterMonster(gameObject);

        }

        private void OnDrawGizmosSelected()
        {
            if (!debugMode)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lineOfSight.position, alertDistance);

            Vector3 leftConeEdge = Quaternion.Euler(0, -fieldOfVisionConeAngle, 0) * lineOfSight.forward;
            Vector3 rightConeEdge = Quaternion.Euler(0, fieldOfVisionConeAngle, 0) * lineOfSight.forward;

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(lineOfSight.position, leftConeEdge * alertDistance);
            Gizmos.DrawRay(lineOfSight.position, rightConeEdge * alertDistance);
            Gizmos.DrawRay(lineOfSight.position, lineOfSight.forward * alertDistance);
        }
    }
}