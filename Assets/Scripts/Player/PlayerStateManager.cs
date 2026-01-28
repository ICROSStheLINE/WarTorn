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
    /// <summary>Player crouching speed in units per second.</summary>
    public float crouchingSpeed = 1.5f;
    /// <summary>Sprint acceleration/deceleration</summary>
    public float sprintAcceleration = 0.1f;

    /// <summary>Smoothing factor for player rotation (0 = instant, 1 = slowest).</summary>
    [Range(0, 1)]
    public float rotationSmoothing = 0.2f;

    /// <summary>Smoothing factor for animation parameter updates.</summary>
    public float animationSmoothing = 0.2f;

    /// <summary> Variable representing whether or not the player is CURRENTLY sprinting (not to be confused with just pressing the sprinting button) </summary>
    [HideInInspector] public bool isSprinting = false;
    /// <summary> Variable representing whether or not the player is CURRENTLY crouching (not to be confused with just pressing the crouching button) </summary>
    [HideInInspector] public bool isCrouching = false;

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
    /// <summary>Owner-authoritative networked player position.</summary>
    public NetworkVariable<Vector3> netPosition = new(writePerm: NetworkVariableWritePermission.Owner);

    /// <summary>Owner-authoritative networked player rotation.</summary>
    public NetworkVariable<Quaternion> netRotation = new(writePerm: NetworkVariableWritePermission.Owner);

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

        // Disable non-owner controllers to prevent conflicts
        if (!IsOwner)
        {
            controller.enabled = false;
            motor.enabled = false;
        }

        if (IsOwner)
        {
            motor.SetPosition(transform.position);
            netPosition.Value = transform.position;
            netRotation.Value = transform.rotation;
        }

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
    /// Updates networked position/rotation for the owner each frame.
    /// </summary>
    private void LateUpdate()
    {
        if (!IsOwner) return;

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
        transform.SetPositionAndRotation(
            Vector3.Lerp(transform.position, netPosition.Value, networkLerpSpeed * Time.deltaTime),
            Quaternion.Slerp(transform.rotation, netRotation.Value, networkLerpSpeed * Time.deltaTime)
        );
    }

    /// <summary>
    /// Applies movement input to the KinematicCharacterController.
    /// Converts Vector2 input and camera rotation into character movement.
    /// </summary>
    /// <param name="move">2D movement input vector.</param>
    /// <param name="cameraRotation">Rotation of the camera to determine forward direction.</param>
    public void ApplyMovement(Vector2 move, Quaternion cameraRotation)
    {
        if (!IsOwner) return;

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
    public void SetSprinting(bool sprinting)
    {
        if (!IsOwner) return;

        isSprinting = sprinting;

        if (sprinting)
        {
            controller.MaxStableMoveSpeed = Mathf.Lerp(controller.MaxStableMoveSpeed, runningSpeed, sprintAcceleration);
        }
        else
        {
            controller.MaxStableMoveSpeed = Mathf.Lerp(controller.MaxStableMoveSpeed, movementSpeed, sprintAcceleration);
        }
    }

    public void SetCrouching(bool crouching)
    {
        isCrouching = crouching;
        
        if (crouching)
        {
            controller.MaxStableMoveSpeed = Mathf.Lerp(controller.MaxStableMoveSpeed, crouchingSpeed, sprintAcceleration);
        }
        else
        {
            controller.MaxStableMoveSpeed = Mathf.Lerp(controller.MaxStableMoveSpeed, movementSpeed, sprintAcceleration);
        }
    }
}
