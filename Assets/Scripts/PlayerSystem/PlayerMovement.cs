using System;
using System.Collections;
using UnityEngine;
using InputSystem;
using EventSystem;
using EventSystem.Events;
using PlayerSystem.StateMachine;

namespace PlayerSystem {
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour {

        // Events
        private Action<Evt_PlayerJumpAction> jumpAction;
        private Action<Evt_PlayerJumpCancel> jumpActionCancel;
        private Action<Evt_PlayerSprintAction> sprintAction;
        private Action<Evt_PlayerSprintCancel> sprintCancelAction;

        [Header("Movement")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float groundDrag;

        private float moveSpeed;
        private float horizontalInput;
        private float verticalInput;
        private bool isSprinting = false;
        private Vector3 moveDir;

        [Header("Jumping")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float airMultiplier;

        private bool canJump;
        private bool isJumpHeld;

        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask groundMask;

        private bool isGrounded;

        //[Header("Slope Handling")]


        [Header("Forward Direction")]
        [SerializeField] private Transform orientation;

        private Rigidbody rb;

        // States
        private PlayerStateMachine stateMachine;

        // Lifecycle
        private void Awake() {
            jumpAction = e => SetJumpHeld(true);
            jumpActionCancel = e => SetJumpHeld(false);
            sprintAction = e => HandleSprint(true);
            sprintCancelAction = e => HandleSprint(false);
        }

        private void OnEnable() {
            EventBus.Subscribe(jumpAction);
            EventBus.Subscribe(jumpActionCancel);
            EventBus.Subscribe(sprintAction);
            EventBus.Subscribe(sprintCancelAction);
        }

        private void Start() {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            stateMachine = GetComponent<PlayerStateMachine>();

            canJump = true;
        }

        private void Update() {
            MoveInput();
            GroundCheck();
        }


        private void OnDisable() {
            EventBus.Unsubscribe(jumpAction);
            EventBus.Unsubscribe(jumpActionCancel);
            EventBus.Unsubscribe(sprintAction);
            EventBus.Unsubscribe(sprintCancelAction);
        }

        private void OnDestroy() {

        }

        // ---------------------------------- Physics Methods ----------------------------------

        /// <summary>
        /// Handles physics for the player
        /// This is called from the PlayerStateMachine to handle physics instead of the FixedUpdate loop monobehaviour
        /// </summary>
        public void ProcessMovementPhysics() {
           HandleMovement();
           SpeedControl();

            if (isJumpHeld) {
                HandleJump();
            }
        }


        // ---------------------------------- Movement Methods ----------------------------------

        /// <summary>
        /// Handles movement input for the player
        /// </summary>
        private void MoveInput() {
            Vector3 input = InputManager.Instance.GetMoveVectorNormalized();
            horizontalInput = input.x;
            verticalInput = input.y;
        }


        /// <summary>
        /// Handles movement of the player
        /// </summary>
        public void HandleMovement() {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (isGrounded) {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
                rb.linearDamping = groundDrag;
            }
            else {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
                rb.linearDamping = 0f;
            }
        }


        /// <summary>
        /// Controls the speed of the player
        /// </summary>
        public void SpeedControl() {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }


        /// <summary>
        /// Handles the bool sprinting of the player 
        /// </summary>
        /// <param name="isSprinting"></param>
        private void HandleSprint(bool isSprinting) {
            this.isSprinting = isSprinting;
        }


        // ---------------------------------- Jumping Methods ----------------------------------

        /// <summary>
        /// Handles the jumping of the player
        /// </summary>
        public void HandleJump() {
            if (isGrounded && canJump) {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                canJump = false;

                StartCoroutine(JumpCooldownTimer());
            }
        }


        /// <summary>
        /// Handles the cooldown of the jump
        /// </summary>
        /// <returns></returns>
        private IEnumerator JumpCooldownTimer() {
            yield return new WaitForSeconds(jumpCooldown);
            canJump = true;
        }


        /// <summary>
        /// Handles the bool jumping of the player
        /// </summary>
        /// <param name="canJump"></param>
        private void SetJumpHeld(bool canJump) {
            isJumpHeld = canJump;
        }

        /// <summary>
        /// Checks if the player is on the ground
        /// </summary>
        private void GroundCheck() {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundMask);
        }

        // ---------------------------------- Getters & Setters ----------------------------------

        /// <summary>
        /// Sets the movement speed
        /// </summary>
        /// <param name="speed"></param>
        public void SetMoveSpeed(float speed) {
            moveSpeed = speed;
        }

        public bool IsGrounded() => isGrounded;
        public bool IsSprinting() => isSprinting;
        public float GetWalkSpeed() => walkSpeed;
        public float GetSprintSpeed() => sprintSpeed;
    }
}