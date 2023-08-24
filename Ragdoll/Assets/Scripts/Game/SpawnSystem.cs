using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : NetworkBehaviour
{
    private Transform[] spawnPoints;
    private List<Transform> availableSpawnPoints = new List<Transform>();

    private NetworkManagerNinjaRagdoll room;
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    private void AssignSpawn(PlayerRespawn playerRespawn)
    {
        Debug.Log("Spawn Assigned");
        playerRespawn.spawnPosition = availableSpawnPoints[0];
        availableSpawnPoints.RemoveAt(0);
        TeleportPlayer(playerRespawn.netIdentity.connectionToClient, playerRespawn.spawnPosition.position);
    }

    private void AssignInitialSpawns()
    {
        foreach(var player in Room.GamePlayers)
        {
            PlayerRespawn playerRespawn = player.GetComponent<PlayerRespawn>();
            AssignSpawn(playerRespawn);
        }
    }

    private void FreeSpawn(PlayerRespawn playerRespawn)
    {
        availableSpawnPoints.Add(playerRespawn.spawnPosition);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        spawnPoints = GetComponentsInChildren<Transform>();
        foreach (Transform t in spawnPoints)
        {
            availableSpawnPoints.Add(t);
        }
        AssignInitialSpawns();
        PlayerRespawn.OnServerStarted += AssignSpawn;
        PlayerRespawn.OnServerStopped += FreeSpawn;
    }

    public override void OnStopServer()
    {
        PlayerRespawn.OnServerStarted -= AssignSpawn;
        PlayerRespawn.OnServerStopped -= FreeSpawn;
        base.OnStopServer();
    }

    [TargetRpc]
    private void TeleportPlayer(NetworkConnectionToClient clientId, Vector3 spawnPosition)
    {
        PlayerRespawn playerRespawn = clientId.identity.GetComponent<PlayerRespawn>();
        playerRespawn.Teleport(spawnPosition);
    }
}
