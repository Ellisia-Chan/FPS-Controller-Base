using EventSystem;
using EventSystem.Events;
using System;
using UnityEngine;

namespace InputSystem {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }

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
        }

        private void OnEnable() {
            inputActions.Enable();

            // Jump Actions
            inputActions.Player.Jump.performed += OnJumpAction;
            inputActions.Player.Jump.canceled += OnJumpActionCancel;

            // Sprint Actions
            inputActions.Player.Sprint.performed += OnSprintAction;
            inputActions.Player.Sprint.canceled += OnSprintCancelAction;
        }

        private void OnDisable() {
            inputActions.Disable();

            // Jump Actions
            inputActions.Player.Jump.performed -= OnJumpAction;
            inputActions.Player.Jump.canceled -= OnJumpActionCancel;

            // Sprint Actions
            inputActions.Player.Sprint.performed -= OnSprintAction;
            inputActions.Player.Sprint.canceled -= OnSprintCancelAction;
        }

        // Event Methods
        private void OnJumpAction(UnityEngine.InputSystem.InputAction.CallbackContext context) => EventBus.Publish(new Evt_PlayerJumpAction());

        private void OnJumpActionCancel(UnityEngine.InputSystem.InputAction.CallbackContext context) => EventBus.Publish(new Evt_PlayerJumpCancel());

        private void OnSprintAction(UnityEngine.InputSystem.InputAction.CallbackContext context) => EventBus.Publish(new Evt_PlayerSprintAction());

        private void OnSprintCancelAction(UnityEngine.InputSystem.InputAction.CallbackContext context) => EventBus.Publish(new Evt_PlayerSprintCancel());


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