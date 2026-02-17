using System;

public static class LocalPlayerRegistry
{
    public static event Action<PlayerStateManager> OnLocalPlayerRegistered;

    public static void Register(PlayerStateManager player)
    {
        OnLocalPlayerRegistered?.Invoke(player);
    }
}
