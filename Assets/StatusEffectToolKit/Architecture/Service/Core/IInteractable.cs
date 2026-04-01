/*
 * Description: Interface used for interactable objects 
 */

using UnityEngine;

namespace Service.Core
{
    public interface IInteractable
    {
        void Interact(GameObject interactor);
    }
}