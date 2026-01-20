using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.Netcode;
using UnityEngine;

public class PlayerStateManager : NetworkBehaviour
{
    public PlayerAirborneState playerAirborneState = new();
    public PlayerCinematicState playerCinematicState = new();
    public PlayerGroundedState playerGroundedState = new();
    private PlayerBaseState currentState;

    public float movementSpeed = 10.0f;
    [Range(0, 1)] public float rotationSmoothing = 0.2f;
    public float animationSmoothing = 0.2f;

    public ThirdPersonCameraController thirdPersonCameraController;
    public Animator animator;

    public NetworkVariable<Vector3> NetPosition = new(writePerm: NetworkVariableWritePermission.Server);
    public NetworkVariable<Quaternion> NetRotation = new(writePerm: NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        var kcc = GetComponent<ExampleCharacterController>();
        var kcm = GetComponent<KinematicCharacterMotor>();

        if (!IsServer)
        {
            kcc.enabled = false;
            kcm.enabled = false;
        }
        else
        {
            ResetServerInputs();
        }

        if (!IsOwner)
        {
            // Disable camera & input for non-owners
            return;
        }

        thirdPersonCameraController.EnableCamera(true);
        Cursor.visible = false;
        currentState = playerGroundedState;
        ChangeState(playerGroundedState);

        NetPosition.OnValueChanged += (oldPos, newPos) =>
        {
            if (!IsServer)
                Debug.Log($"[CLIENT] Received pos {newPos}");
        };
    }

    void Update()
    {
        if (IsOwner)
        {
            OwnerUpdate();
        }

        if (!IsOwner)
        {
            ClientUpdate();
        }
    }

    void OwnerUpdate()
    {
        currentState.UpdateState(this);
    }

    void ClientUpdate()
    {
        ApplyRemoteState();
    }

    void ApplyRemoteState()
    {
        if (IsServer) return;

        transform.SetPositionAndRotation(Vector3.Lerp(
            transform.position,
            NetPosition.Value,
            15f * Time.deltaTime
        ), Quaternion.Slerp(
            transform.rotation,
            NetRotation.Value,
            15f * Time.deltaTime
        ));
    }


    void LateUpdate()
    {
        if (!IsServer) return;

        NetPosition.Value = transform.position;
        NetRotation.Value = transform.rotation;
    }


    public void ChangeState(PlayerBaseState state)
    {
        currentState?.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    [Rpc(SendTo.Server)]
    public void SubmitMovementInputServerRpc(Vector2 move, Quaternion cameraRotation)
    {
        ApplyMovement(move, cameraRotation);
    }

    private void ApplyMovement(Vector2 move, Quaternion cameraRotation)
    {
        var controller = GetComponent<ExampleCharacterController>();

        PlayerCharacterInputs inputs = new()
        {
            MoveAxisForward = move.y,
            MoveAxisRight = move.x,
            CameraRotation = cameraRotation
        };

        controller.SetInputs(ref inputs);
    }

    private void ResetServerInputs()
    {
        var motor = GetComponent<KinematicCharacterMotor>();

        var controller = GetComponent<ExampleCharacterController>();

        PlayerCharacterInputs zeroInputs = new()
        {
            MoveAxisForward = 0f,
            MoveAxisRight = 0f,
            CameraRotation = Quaternion.identity
        };

        controller.SetInputs(ref zeroInputs);
    }

}
