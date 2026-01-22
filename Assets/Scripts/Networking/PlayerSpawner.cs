using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Handles spawning player objects when clients connect to the server.
/// Uses Netcode for GameObjects connection approval to assign
/// a custom spawn position for each player.
/// </summary>
[RequireComponent(typeof(NetworkManager))]
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// Index used to offset each player's spawn position
    /// so players do not spawn on top of each other.
    /// </summary>
    private int playerIndex = 0;

    /// <summary>
    /// Called when the GameObject is initialized.
    /// Registers the connection approval callback on the NetworkManager.
    /// </summary>
    void Awake()
    {
        var networkManager = GetComponent<NetworkManager>();
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
    }

    /// <summary>
    /// Connection approval callback invoked whenever a client attempts to connect to the server.
    /// Approves the connection, creates the player object, and assigns a spawn position.
    /// 
    /// Spawn logic:
    /// - Attempts to spawn the player at (0,0,0) by default.
    /// - Checks all existing player objects to ensure the new player does not spawn
    ///   within a minimum distance (1 unit) of any other player.
    /// - If the default position is occupied, it increments the spawn position diagonally
    ///   (by +1 on X and Z) until a free position is found or a maximum number of attempts
    ///   (100) is reached to prevent infinite loops.
    /// 
    /// This guarantees that players do not spawn on top of each other while keeping the
    /// first player at the origin when possible.
    /// </summary>
    /// <param name="request">
    /// Data sent by the connecting client (e.g., payload, client ID).
    /// </param>
    /// <param name="response">
    /// Server response controlling approval, player creation, and spawn transform.
    /// </param>

    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        // Allow connection
        response.Approved = true;

        // Spawn player automatically
        response.CreatePlayerObject = true;

        // Default spawn position
        Vector3 spawnPos = Vector3.zero;

        // Check existing players and make sure we don't spawn too close
        float minDistance = 1f; // distance threshold
        bool positionBlocked = true;
        int attempt = 0;

        while (positionBlocked && attempt < 100) // avoid infinite loop
        {
            positionBlocked = false;
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                if (client.PlayerObject != null)
                {
                    Vector3 existingPos = client.PlayerObject.transform.position;
                    if (Vector3.Distance(existingPos, spawnPos) < minDistance)
                    {
                        positionBlocked = true;
                        // move to next spot (simple strategy: +1 on x and z)
                        spawnPos += new Vector3(1, 0, 1);
                        break; // check again against all players
                    }
                }
            }
            attempt++;
        }

        response.Position = spawnPos;
        response.Rotation = Quaternion.identity;
    }
}
