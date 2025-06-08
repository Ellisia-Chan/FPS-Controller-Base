using EventSystem;
using EventSystem.Events;
using System;
using UnityEngine;

namespace InputSystem {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }

        private Action<UnityEngine.InputSystem.InputAction.CallbackContext> onJumpAction, onJumpActionCancel, onSprintAction, onSprintCancelAction;

        private InputSystem_Actions inputActions;

        // Lifecycle

        private void Awake() {
            if (Instance != null) {
                Debug.LogWarning("InputManager: Instance already exists");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            inputActions = new InputSystem_Actions();

            onJumpAction = e => EventBus.Publish(new Evt_PlayerJumpAction());
            onJumpActionCancel = e => EventBus.Publish(new Evt_PlayerJumpCancel());
            onSprintAction = e => EventBus.Publish(new Evt_PlayerSprintAction());
            onSprintCancelAction = e => EventBus.Publish(new Evt_PlayerSprintCancel());
        }

        private void OnEnable() {
            inputActions.Enable();

            // Jump Actions
            inputActions.Player.Jump.performed += onJumpAction;
            inputActions.Player.Jump.canceled += onJumpActionCancel;

            // Sprint Actions
            inputActions.Player.Sprint.performed += onSprintAction;
            inputActions.Player.Sprint.canceled += onSprintCancelAction;
        }

        private void OnDisable() {
            inputActions.Disable();

            // Jump Actions
            inputActions.Player.Jump.performed -= onJumpAction;
            inputActions.Player.Jump.canceled -= onJumpActionCancel;

            // Sprint Actions
            inputActions.Player.Sprint.performed -= onSprintAction;
            inputActions.Player.Sprint.canceled -= onSprintCancelAction;
        }


        // Methods

        /// <summary>
        /// Returns mouse axis vector
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMouseAxisVector() {
            return inputActions.Player.Look.ReadValue<Vector2>();
        }


        /// <summary>
        /// Returns normalized move vector
        /// </summary>
        /// <returns></returns>
        public Vector3 GetMoveVectorNormalized() {
            return inputActions.Player.Move.ReadValue<Vector2>().normalized;
        }
    }
}