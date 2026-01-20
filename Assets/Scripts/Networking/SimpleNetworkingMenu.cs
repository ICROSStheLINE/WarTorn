using System.Linq;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Add this component to the same GameObject as
/// the NetworkManager component.
/// </summary>
public class SimpleNetworkingMenu : MonoBehaviour
{
    private NetworkManager m_NetworkManager;

    public Camera menuCamera;

    private void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            // SubmitNewPosition(); 
        }

        GUILayout.EndArea();
    }

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

    private void StatusLabels()
    {
        var mode = m_NetworkManager.IsHost ?
            "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    // private void SubmitNewPosition()
    // {
    //     if (GUILayout.Button(m_NetworkManager.IsServer ? "Move" : "Request Position Change"))
    //     {
    //         if (m_NetworkManager.IsServer && !m_NetworkManager.IsClient)
    //         {
    //             foreach (ulong uid in m_NetworkManager.ConnectedClientsIds)
    //                 m_NetworkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<NetworkPlayer>().Move();
    //         }
    //         else
    //         {
    //             var playerObject = m_NetworkManager.SpawnManager.GetLocalPlayerObject();
    //             var player = playerObject.GetComponent<NetworkPlayer>();
    //             player.Move();
    //         }
    //     }
    // }
}