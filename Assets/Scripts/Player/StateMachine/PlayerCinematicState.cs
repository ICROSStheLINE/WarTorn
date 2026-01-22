/// <summary>
/// Represents a cinematic state for the player.
/// While in this state, the player's normal controls and camera
/// are disabled, typically for cutscenes or scripted sequences.
/// </summary>
public class PlayerCinematicState : PlayerBaseState
{
    /// <summary>
    /// Called when entering the cinematic state.
    /// Disables the third-person camera controller to prevent player control.
    /// </summary>
    /// <param name="player">
    /// The state manager controlling the player and handling state transitions.
    /// </param>
    public override void EnterState(PlayerStateManager player)
    {
        player.CameraController.enabled = false;
    }

    /// <summary>
    /// Called when exiting the cinematic state.
    /// Re-enables the third-person camera controller to restore player control.
    /// </summary>
    /// <param name="player">
    /// The state manager controlling the player and handling state transitions.
    /// </param>
    public override void ExitState(PlayerStateManager player)
    {
        player.CameraController.enabled = true;
    }
}
