using System.Linq;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Simple immediate-mode GUI menu for starting Netcode for GameObjects
/// in Host, Client, or Server mode.
/// Displays connection status once networking has started.
/// </summary>
public class SimpleNetworkingMenu : MonoBehaviour
{
    /// <summary>
    /// Cached reference to the active NetworkManager.
    /// </summary>
    private NetworkManager m_NetworkManager;

    /// <summary>
    /// Camera used to render the main menu.
    /// Disabled once networking starts.
    /// </summary>
    public Camera menuCamera;

    /// <summary>
    /// Called when the GameObject is initialized.
    /// Retrieves and caches the NetworkManager component.
    /// </summary>
    private void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
    }

    /// <summary>
    /// Unity IMGUI callback used to render the networking menu.
    /// Shows start buttons when not connected, otherwise shows status labels.
    /// </summary>
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        // If networking has not started yet, show start buttons
        if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
        {
            StartButtons();
        }
        // Otherwise, show current networking status
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    /// <summary>
    /// Draws buttons that allow the user to start the application
    /// as a Host, Client, or dedicated Server.
    /// </summary>
    private void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            menuCamera.enabled = false;
            m_NetworkManager.StartHost();
        }

        if (GUILayout.Button("Client"))
        {
            menuCamera.enabled = false;
            m_NetworkManager.StartClient();
        }

        if (GUILayout.Button("Server"))
        {
            menuCamera.enabled = false;
            m_NetworkManager.StartServer();
        }
    }

    /// <summary>
    /// Displays current networking information such as
    /// transport type and running mode (Host, Server, or Client).
    /// </summary>
    private void StatusLabels()
    {
        // Determine the current networking mode
        var mode = m_NetworkManager.IsHost
            ? "Host"
            : m_NetworkManager.IsServer
                ? "Server"
                : "Client";

        GUILayout.Label(
            "Transport: " +
            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name
        );

        GUILayout.Label("Mode: " + mode);
    }
}
