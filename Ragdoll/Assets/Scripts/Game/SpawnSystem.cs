using Cinemachine.Utility;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : NetworkBehaviour
{
    private List<Vector3> availableSpawnPoints = new List<Vector3>();

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
        playerRespawn.spawnPosition = availableSpawnPoints[0];
        availableSpawnPoints.RemoveAt(0);
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
        Transform[] spawnPoints = GetComponentsInChildren<Transform>();
        foreach (Transform t in spawnPoints)
        {
            if (t != this.transform)
                availableSpawnPoints.Add(t.position);
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
}
