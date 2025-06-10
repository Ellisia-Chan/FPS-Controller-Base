using UnityEngine;

namespace PlayerSystem.StateMachine.States {
    public class PlayerRunningState : PlayerBaseState {
        public override void EnterState(PlayerStateMachine player) {
            player.playerMovement.SetMoveSpeed(player.playerMovement.GetSprintSpeed());
            Debug.Log("Running");
        }

        public override void UpdateState(PlayerStateMachine player) {
            if (!player.playerMovement.IsGrounded()) {
                player.SwitchState(player.PlayerJumpState);
            }
            else if (!player.playerMovement.IsSprinting()) {
                player.SwitchState(player.PlayerWalkingState);
            }
        }

        public override void FixedUpdateState(PlayerStateMachine player) {
            player.playerMovement.ProcessMovementPhysics();
        }

        public override void ExitState(PlayerStateMachine player) {
        }
    }
}