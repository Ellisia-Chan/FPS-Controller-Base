using EventSystem;
using EventSystem.Events;
using UnityEngine;

using Interface;

namespace PlayerSystem
{
    /// <summary>
    /// Handles the player's interaction with IInteractable objects.
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private Transform interactionSource;
        [SerializeField] private float interactionRange = 2f;

        private IInteractable currentInteractable;
        private IInteractable lastInteractable;

        private void OnEnable()
        {
            EventBus.Subscribe<Evt_PlayerInteractAction>(OnInteractAction);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<Evt_PlayerInteractAction>(OnInteractAction);
        }

        private void Update()
        {
            CheckForInteractable();
        }

        /// <summary>
        /// Checks for interactable objects in front of the player.
        /// </summary>
        private void CheckForInteractable()
        {
            Ray r = new Ray(interactionSource.position, interactionSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactionRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    currentInteractable = interactable;

                    if (currentInteractable != lastInteractable)
                    {
                        lastInteractable?.OnLoseFocus();
                        currentInteractable.OnFocus();
                    }
                }
                else
                {
                    ClearInteractable();
                }
            }
            else
            {
                ClearInteractable();
            }

            lastInteractable = currentInteractable;
        }

        /// <summary>
        /// Clears the current interactable if the player is not looking at one.
        /// </summary>
        private void ClearInteractable()
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }
        }

        /// <p_summary>
        /// Called when the player presses the interact button.
        /// </p_summary>
        /// <param name="e"></param>
        private void OnInteractAction(Evt_PlayerInteractAction e)
        {
            currentInteractable?.OnInteract(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(interactionSource.position, interactionSource.forward * interactionRange);
        }
    }
}
