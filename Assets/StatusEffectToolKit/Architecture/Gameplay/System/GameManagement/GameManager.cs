/*
 * Description: Handles global game logic specific to gameplay
 */

using Service.Framework.Health;
using Service.Framework.StatusSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Gameplay.System
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnLevelLoaded = new UnityEvent();

        public static GameManager Instance;

        private HealthHandler playerHealth;

        private float waitToStart = 1f;
        private WaitForSeconds waitToStartEnumerator;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            waitToStartEnumerator = new WaitForSeconds(waitToStart);

            playerHealth = ReferenceRegistry.Instance.Player.GetComponent<HealthHandler>();

            playerHealth.OnHealthUpdated.AddListener(OnPlayerDamaged);

            StartInitializing();
        }

        /// <summary>
        /// Start initializing.  Useful for scene loading, game state changes, etc.
        /// </summary>
        public void StartInitializing()
        {
            StatusManager.Instance.CleanUpActionHandlers();
            StatusManager.Instance.SetActionHandlersUpdateState(true);
            StartCoroutine(Initialize());
        }

        /// <summary>
        /// Do any delay logic
        /// </summary>
        /// <returns></returns>
        private IEnumerator Initialize()
        {
            //do a brief delay before giving the player control for a smoother flow
            ReferenceRegistry.Instance.Player.RemoveControl();            
            yield return waitToStartEnumerator;
            ReferenceRegistry.Instance.Player.GiveControl();
            
        }

        private void OnPlayerDamaged(float value, GameObject obj)
        {
            if (playerHealth.State == HealthHandler.CurrentState.Dead)
            {
                //set the status level to the max, of which should always be set to the Death status effect
                StatusManager.Instance.SetStatusLevel(StatusManager.Instance.StatusSettings.statusLevels.Count - 1);
            }
        }

        public void OnLoadNewLevel(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            OnLevelLoaded.Invoke();
        }

        private void OnDestroy()
        {
            playerHealth.OnHealthUpdated.RemoveListener(OnPlayerDamaged);

        }
    }
}