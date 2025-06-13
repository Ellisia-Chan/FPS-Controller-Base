using EventSystem;
using EventSystem.Events;
using InputSystem;
using System;
using System.Collections;
using UnityEngine;

namespace PlayerSystem {
    public class PlayerMovement : MonoBehaviour {

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
        public MovementState moveState;
        public enum MovementState {
            walking,
            sprinting,
            air
        }

        // Lifecycle
        private void OnEnable() {
            EventBus.Subscribe<Evt_PlayerJumpAction>(JumpActionEvent);
            EventBus.Subscribe<Evt_PlayerJumpCancel>(JumpCancelActionEvent);
            EventBus.Subscribe<Evt_PlayerSprintAction>(SprintActionEvent);
            EventBus.Subscribe<Evt_PlayerSprintCancel>(SprintCancelActionEvent);
        }

        private void Start() {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            canJump = true;
        }

        private void Update() {
            StateHandler();
            MoveInput();
            GroundCheck();

        }

        private void FixedUpdate() {
            HandleMovement();
            SpeedControl();

            if (isJumpHeld) {
                HandleJump();
            }
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_PlayerJumpAction>(JumpActionEvent);
            EventBus.Unsubscribe<Evt_PlayerJumpCancel>(JumpCancelActionEvent);
            EventBus.Unsubscribe<Evt_PlayerSprintAction>(SprintActionEvent);
            EventBus.Unsubscribe<Evt_PlayerSprintCancel>(SprintCancelActionEvent);
        }

        private void OnDestroy() {

        }

        // Event Methods
        private void JumpActionEvent(Evt_PlayerJumpAction e) { SetJumpHeld(true); }
        private void JumpCancelActionEvent(Evt_PlayerJumpCancel e) { SetJumpHeld(false); }

        private void SprintActionEvent(Evt_PlayerSprintAction e) { HandleSprint(true);  }
        private void SprintCancelActionEvent(Evt_PlayerSprintCancel e) { HandleSprint(false); }



        // Methods

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
        private void HandleMovement() {
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
        private void SpeedControl() {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }


        /// <summary>
        /// Checks if the player is on the ground
        /// </summary>
        private void GroundCheck() {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundMask);
        }


        /// <summary>
        /// Handles the jumping of the player
        /// </summary>
        private void HandleJump() {
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
        private void SetJumpHeld(bool canJump) => isJumpHeld = canJump;


        /// <summary>
        /// Resets the bool jumping of the player
        /// </summary>
        private void ResetJump() {
            canJump = true;
        }


        /// <summary>
        /// Handles the bool sprinting of the player 
        /// </summary>
        /// <param name="isSprinting"></param>
        private void HandleSprint(bool isSprinting) {
            this.isSprinting = isSprinting;
        }


        /// <summary>
        /// Handles the state of the player movement
        /// </summary>
        private void StateHandler() {
            if (isGrounded && isSprinting) {
                SetState(MovementState.sprinting);
            }
            else if (isGrounded && !isSprinting) {
                SetState(MovementState.walking);
            }
            else {
                SetState(MovementState.air);
            }
        }


        /// <summary>
        /// Sets the state of the player movement
        /// </summary>
        /// <param name="newState"></param>
        private void SetState(MovementState newState) {
            if (moveState == newState) return;

            moveState = newState;

            switch (moveState) {
                case MovementState.walking:
                    moveSpeed = walkSpeed;
                    break;
                case MovementState.sprinting:
                    moveSpeed = sprintSpeed;
                    break;
                case MovementState.air:
                    moveSpeed = walkSpeed;
                    break;
                default:
                    moveSpeed = walkSpeed;
                    break;
            }
        }


        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * playerHeight * 0.5f);
        }
    }
}