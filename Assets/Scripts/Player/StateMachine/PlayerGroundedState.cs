using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerBaseState
{
    Animator animator;
    InputAction moveAction;

    public override void EnterState(PlayerStateManager player)
    {
        if (!player.IsOwner) return;

        moveAction = InputSystem.actions.FindAction("Move");
        animator = player.animator;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.IsOwner) return;

        Vector2 moveVector = moveAction.ReadValue<Vector2>();

        SendMovementToServer(player, moveVector);
        UpdateAnimations(player, moveVector);
    }

    private void SendMovementToServer(PlayerStateManager player, Vector2 moveVector)
    {
        if (moveVector.sqrMagnitude < 0.0001f)
        {
            moveVector = Vector2.zero;
        }


        Quaternion targetRotation = Quaternion.Euler(
            0f,
            player.thirdPersonCameraController.rotation.eulerAngles.y,
            0f
        );

        player.SubmitMovementInputServerRpc(moveVector, targetRotation);
    }

    private void UpdateAnimations(PlayerStateManager player, Vector2 moveVector)
    {
        float newX = Mathf.Lerp(
            animator.GetFloat("VelocityX"),
            moveVector.x,
            player.animationSmoothing
        );

        float newY = Mathf.Lerp(
            animator.GetFloat("VelocityY"),
            moveVector.y,
            player.animationSmoothing
        );

        animator.SetFloat("VelocityX", newX);
        animator.SetFloat("VelocityY", newY);
    }
}
