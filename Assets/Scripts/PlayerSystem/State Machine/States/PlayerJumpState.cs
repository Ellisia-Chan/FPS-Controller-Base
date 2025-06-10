using UnityEngine;

namespace PlayerSystem.StateMachine.States {
    public class PlayerJumpState : PlayerBaseState {
        public override void EnterState(PlayerStateMachine player) {
            player.playerMovement.SetMoveSpeed(player.playerMovement.GetWalkSpeed());
            Debug.Log("Jump");
        }
        public override void UpdateState(PlayerStateMachine player) {
            if (player.playerMovement.IsGrounded()) {
                if (player.playerMovement.IsSprinting()) {
                    player.SwitchState(player.PlayerRunningState);
                } else {
                    player.SwitchState(player.PlayerWalkingState);
                }
            }
            
        }
        public override void FixedUpdateState(PlayerStateMachine player) {
            player.playerMovement.ProcessMovementPhysics();
        }

        public override void ExitState(PlayerStateMachine player) {
        }

    }
}
