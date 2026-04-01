/*
 * Description: Generic script that serves as the handshake for interactable objects
 */

using Service.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Service.Framework
{
    public class PlayerInteractor : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnInteracted = new UnityEvent();

        public IInteractable CurrentInteractable { get; private set; }

        public void SetCurrentInteractable(IInteractable interactable) => CurrentInteractable = interactable;

        public void ClearCurrentInteractable(IInteractable interactable)
        {
            if (CurrentInteractable == interactable)
            {
                CurrentInteractable = null;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            //pass this game object so systems know which object is being interacted with
            CurrentInteractable?.Interact(gameObject);
            OnInteracted.Invoke();
        }
    }
}