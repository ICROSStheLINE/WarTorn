using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerAirborneState playerAirborneState = new();
    public PlayerCinematicState playerCinematicState = new();
    public PlayerGroundedState playerGroundedState = new();
    private PlayerBaseState currentState;

    public float movementSpeed = 10.0f;
    [Range(0, 1)]
    public float rotationSmoothing = 0.2f;

    public float animationSmoothing = 0.2f;

    public ThirdPersonCameraController thirdPersonCameraController;
    public Animator animator;

    void Awake()
    {
        Cursor.visible = false;
        currentState = playerGroundedState;
        ChangeState(playerGroundedState);
    }
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(PlayerBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}
