using UnityEngine;
using PlayerSystem.StateMachine.States;

namespace PlayerSystem.StateMachine {
    public class PlayerStateMachine : MonoBehaviour {
        [HideInInspector]
        public PlayerMovement playerMovement;

        private PlayerBaseState currentState;

        public PlayerWalkingState PlayerWalkingState = new PlayerWalkingState();
        public PlayerRunningState PlayerRunningState = new PlayerRunningState();
        public PlayerJumpState PlayerJumpState = new PlayerJumpState();

        private void Awake() {
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start() {
            currentState = PlayerWalkingState;
            currentState.EnterState(this);
        }

        private void Update() {
            currentState.UpdateState(this);
        }

        private void FixedUpdate() {
            currentState.FixedUpdateState(this);
        }

        public void SwitchState(PlayerBaseState newState) {
            currentState.ExitState(this);
            currentState = newState;
            currentState.EnterState(this);
        }
    }
}