using System.Collections;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Manages the player's finite state machine (FSM) states, movement, 
/// animations, and network synchronization.
/// Integrates with KinematicCharacterController for movement
/// and Unity Netcode for GameObjects for multiplayer.
/// </summary>
[RequireComponent(typeof(ExampleCharacterController))]
[RequireComponent(typeof(KinematicCharacterMotor))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerStateManager : NetworkBehaviour
{
    // --- States ---
    /// <summary>State when the player is in the air (jumping or falling).</summary>
    public PlayerAirborneState playerAirborneState = new();

    /// <summary>State when the player is in a cinematic (cutscene) sequence.</summary>
    public PlayerCinematicState playerCinematicState = new();

    /// <summary>State when the player is on the ground and can move freely.</summary>
    public PlayerGroundedState playerGroundedState = new();

    /// <summary>The currently active player state.</summary>
    private PlayerBaseState currentState;

    // --- Movement ---
    [Header("Movement Settings")]
    /// <summary>Player walking speed in units per second.</summary>
    public float movementSpeed = 2f;
    /// <summary>Player running speed in units per second.</summary>
    public float runningSpeed = 8f;

    /// <summary>Smoothing factor for player rotation (0 = instant, 1 = slowest).</summary>
    [Range(0, 1)]
    public float rotationSmoothing = 0.2f;

    /// <summary>Smoothing factor for animation parameter updates.</summary>
    public float animationSmoothing = 0.2f;

    // --- References ---
    [Header("References")]
    [SerializeField] private ThirdPersonCameraController thirdPersonCameraController;

    /// <summary>Read-only reference to the third-person camera controller.</summary>
    public ThirdPersonCameraController CameraController => thirdPersonCameraController;

    [SerializeField] private Animator animator;

    /// <summary>Read-only reference to the player's animator component.</summary>
    public Animator Animator => animator;

    private ExampleCharacterController controller;
    private KinematicCharacterMotor motor;

    // --- Networking ---
    [Header("Networking")]
    /// <summary>Server-authoritative networked player position.</summary>
    public NetworkVariable<Vector3> netPosition = new(writePerm: NetworkVariableWritePermission.Server);

    /// <summary>Server-authoritative networked player rotation.</summary>
    public NetworkVariable<Quaternion> netRotation = new(writePerm: NetworkVariableWritePermission.Server);

    /// <summary>Speed at which remote players interpolate their position and rotation.</summary>
    [SerializeField] private float networkLerpSpeed = 15f;

    // --- Unity Events ---
    /// <summary>
    /// Called when the player object spawns on the network.
    /// Initializes components, enables camera for owner, and sets initial state.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        // Cache components
        controller = GetComponent<ExampleCharacterController>();
        motor = GetComponent<KinematicCharacterMotor>();

        // Disable non-server controllers to prevent conflicts
        if (!IsServer)
        {
            controller.enabled = false;
            motor.enabled = false;
        }

        motor.SetPosition(transform.position);

        if (!IsOwner) return;

        // Enable camera and hide cursor for the local player
        thirdPersonCameraController.EnableCamera(true);
        Cursor.visible = false;

        // Start in grounded state
        ChangeState(playerGroundedState);
    }

    /// <summary>
    /// Called every frame. Updates current state for the owner
    /// or interpolates position/rotation for remote players.
    /// </summary>
    private void Update()
    {
        if (IsOwner) currentState.UpdateState(this);
        else ApplyRemoteState();
    }

    /// <summary>
    /// Updates networked position/rotation for the server each frame.
    /// </summary>
    private void LateUpdate()
    {
        if (!IsServer) return;

        netPosition.Value = transform.position;
        netRotation.Value = transform.rotation;
    }

    // --- State Management ---
    /// <summary>
    /// Changes the current FSM state to a new state.
    /// Calls ExitState on the old state and EnterState on the new one.
    /// </summary>
    /// <param name="newState">The state to transition into.</param>
    public void ChangeState(PlayerBaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    // --- Networking ---
    /// <summary>
    /// Applies position and rotation updates for non-owner players.
    /// Interpolates between current transform and networked values.
    /// </summary>
    private void ApplyRemoteState()
    {
        if (IsServer) return;

        transform.SetPositionAndRotation(
            Vector3.Lerp(transform.position, netPosition.Value, networkLerpSpeed * Time.deltaTime),
            Quaternion.Slerp(transform.rotation, netRotation.Value, networkLerpSpeed * Time.deltaTime)
        );
    }

    /// <summary>
    /// Server RPC called by the owner to submit movement input.
    /// </summary>
    /// <param name="move">2D movement vector from input.</param>
    /// <param name="cameraRotation">Current camera rotation for direction.</param>
    [ServerRpc]
    public void SubmitMovementInputServerRpc(Vector2 move, Quaternion cameraRotation)
    {
        ApplyMovement(move, cameraRotation);
    }

    /// <summary>
    /// Applies movement input to the KinematicCharacterController.
    /// Converts Vector2 input and camera rotation into character movement.
    /// </summary>
    /// <param name="move">2D movement input vector.</param>
    /// <param name="cameraRotation">Rotation of the camera to determine forward direction.</param>
    private void ApplyMovement(Vector2 move, Quaternion cameraRotation)
    {
        PlayerCharacterInputs inputs = new()
        {
            MoveAxisForward = move.y,
            MoveAxisRight = move.x,
            CameraRotation = cameraRotation
        };

        controller.SetInputs(ref inputs);
    }

    /// <summary>
    /// Changes <see cref="ExampleCharacterController.MaxStableMoveSpeed "/> speed based on the sprinting parameter
    /// </summary>
    /// <param name="sprinting">Whether the player is sprinting or not</param>
    [ServerRpc]
    public void SetSprintingServerRpc(bool sprinting)
    {
        if (sprinting)
        {
            controller.MaxStableMoveSpeed = runningSpeed;
        }
        else
        {
            controller.MaxStableMoveSpeed = movementSpeed;
        }
    }

    [ServerRpc]
    public void SetCrouchingServerRpc(bool crouching)
    {
        
    }
}
