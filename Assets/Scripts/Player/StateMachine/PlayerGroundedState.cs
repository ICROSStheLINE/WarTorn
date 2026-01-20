using System;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerBaseState
{
    Animator animator;
    ExampleCharacterController characterController;
    InputAction moveAction;
    public override void EnterState(PlayerStateManager player)
    {
        moveAction = InputSystem.actions.FindAction("Move");
        animator = player.animator;
        characterController = player.GetComponent<ExampleCharacterController>();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        UpdateMovement(player, moveVector);
        UpdateAnimations(player, moveVector);
    }

    private void UpdateMovement(PlayerStateManager player, Vector2 moveVector)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, player.thirdPersonCameraController.rotation.eulerAngles.y, 0f);
        PlayerCharacterInputs characterInputs = new()
        {
            MoveAxisForward = moveVector.y,
            MoveAxisRight = moveVector.x,
            CameraRotation = targetRotation
        };
        characterController.SetInputs(ref characterInputs);
    }

    private void UpdateAnimations(PlayerStateManager player, Vector2 moveVector)
    {
        float newX = Mathf.Lerp(animator.GetFloat("VelocityX"), moveVector.x, player.animationSmoothing);
        float newY = Mathf.Lerp(animator.GetFloat("VelocityY"), moveVector.y, player.animationSmoothing);
        animator.SetFloat("VelocityX", newX);
        animator.SetFloat("VelocityY", newY);
    }
}