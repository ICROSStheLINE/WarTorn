using Unity.Netcode;
using UnityEngine;

public class ChangeRemoteIconColor : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private Color remotePlayerColor = Color.yellow;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        if (iconRenderer == null)
        {
            Debug.LogWarning("Icon Renderer not assigned on " + gameObject.name);
            return;
        }

        // Change color to indicate remote player
        iconRenderer.color = remotePlayerColor;
    }
}
