using UnityEngine;

namespace PlayerSystem.StateMachine.States {
    public class PlayerWalkingState : PlayerBaseState {
        public override void EnterState(PlayerStateMachine player) {
            player.playerMovement.SetMoveSpeed(player.playerMovement.GetWalkSpeed());
            Debug.Log("Walking");
        }
        public override void UpdateState(PlayerStateMachine player) {
            if (!player.playerMovement.IsGrounded()) {
                player.SwitchState(player.PlayerJumpState);
            }
            else if (player.playerMovement.IsSprinting()) {
                player.SwitchState(player.PlayerRunningState);
            }
        }
        public override void FixedUpdateState(PlayerStateMachine player) {
            player.playerMovement.ProcessMovementPhysics();
        }

        public override void ExitState(PlayerStateMachine player) {
            
        }

    }
}
