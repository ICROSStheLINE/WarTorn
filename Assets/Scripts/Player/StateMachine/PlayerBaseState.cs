using UnityEngine;

/// <summary>
/// Base class for all player finite state machine (FSM) states.
/// Provides a common interface for player behavior and state transitions.
/// </summary>
public abstract class PlayerBaseState
{
    /// <summary>
    /// Called when the state becomes active.
    /// Initialize state-specific logic here (e.g., animations, timers).
    /// </summary>
    /// <param name="player">
    /// The state manager controlling the player and handling state transitions.
    /// </param>
    public virtual void EnterState(PlayerStateManager player) { }

    /// <summary>
    /// Called once per frame while this state is active.
    /// Update state-specific behavior here (e.g., movement, input handling).
    /// </summary>
    /// <param name="player">
    /// The state manager controlling the player and handling state transitions.
    /// </param>
    public virtual void UpdateState(PlayerStateManager player) { }

    /// <summary>
    /// Called when the state is about to be exited.
    /// Clean up or reset state-specific logic here.
    /// </summary>
    /// <param name="player">
    /// The state manager controlling the player and handling state transitions.
    /// </param>
    public virtual void ExitState(PlayerStateManager player) { }
}
