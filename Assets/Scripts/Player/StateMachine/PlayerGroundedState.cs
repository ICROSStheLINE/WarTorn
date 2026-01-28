using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents the player's grounded state.
/// Handles movement input, applies owner-authoritative movement,
/// and updates animations while the player is on the ground.
/// </summary>
public class PlayerGroundedState : PlayerBaseState
{
    /// <summary>
    /// Cached reference to the player's Animator component.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Input action for reading movement input from the player.
    /// </summary>
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    private static readonly float AnimationThreshold = 0.5f;

    /// <summary>
    /// Called when entering the grounded state.
    /// Initializes the movement input action and animator references.
    /// </summary>
    /// <param name="player">
    /// The state manager controlling the player and handling state transitions.
    /// </param>
    public override void EnterState(PlayerStateManager player)
    {
        if (!player.IsOwner) return;

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        animator = player.Animator;
    }

    /// <summary>
    /// Called every frame while the player remains in the grounded state.
    /// Reads player input, sends movement to the server, and updates animations.
    /// </summary>
    /// <param name="player">
    /// The state manager controlling the player and handling state transitions.
    /// </param>
    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.IsOwner) return;

        Vector2 moveVector = moveAction.ReadValue<Vector2>();

        ApplyMovementFromInput(player, moveVector);
        bool isAttemptingSprint = sprintAction.IsPressed() && moveVector.y > AnimationThreshold && Mathf.Abs(moveVector.x) < AnimationThreshold;
        bool isAttemptingCrouch = crouchAction.IsPressed();
        // account for sprint in animation
        var animationMoveVector = new Vector2(moveVector.x, isAttemptingSprint ? moveVector.y * 2 : moveVector.y);
        UpdateAnimations(player, animationMoveVector, isAttemptingCrouch);
        ApplySprintingState(player, isAttemptingSprint && !isAttemptingCrouch);
        ApplyCrouchingState(player, isAttemptingCrouch);
    }

    /// <summary>
    /// Applies the player's movement input and target rotation locally.
    /// </summary>
    /// <param name="player">The player state manager.</param>
    /// <param name="moveVector">The movement input vector from the player.</param>
    private void ApplyMovementFromInput(PlayerStateManager player, Vector2 moveVector)
    {
        if (moveVector.sqrMagnitude < 0.0001f)
        {
            moveVector = Vector2.zero;
        }

        // Calculate the target rotation based on camera direction
        Quaternion targetRotation = Quaternion.Euler(
            0f,
            player.CameraController.CameraTransform.rotation.eulerAngles.y,
            0f
        );

        // Apply movement input locally (owner authoritative)
        player.ApplyMovement(moveVector, targetRotation);
    }

    /// <summary>
    /// Updates animator parameters based on player input with smoothing.
    /// </summary>
    /// <param name="player">The player state manager.</param>
    /// <param name="moveVector">The movement input vector from the player.</param>
    private void UpdateAnimations(PlayerStateManager player, Vector2 moveVector, bool crouching)
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
        animator.SetBool("Crouching", crouching);
    }

    private void ApplySprintingState(PlayerStateManager player, bool sprinting)
    {
        player.SetSprinting(sprinting);
    }

    private void ApplyCrouchingState(PlayerStateManager player, bool crouching)
    {
        player.SetCrouching(crouching);
    }
}
