public class PlayerCinematicState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.thirdPersonCameraController.enabled = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.thirdPersonCameraController.enabled = true;
    }
}