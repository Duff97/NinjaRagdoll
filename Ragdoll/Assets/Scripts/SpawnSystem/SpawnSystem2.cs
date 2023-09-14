using Cinemachine.Utility;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem2 : NetworkBehaviour
{
    public Transform hunterSpawnPoint;
    public List<Transform> ninjaSpawnPoints;

    private NetworkManagerNinjaRagdoll room;
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    private void SpawnNinja(PlayerSpawn playerSpawn)
    {
        int index = Random.Range(0, ninjaSpawnPoints.Count);
        playerSpawn.Spawn(ninjaSpawnPoints[index].position);
    }

    private void SpawnHunter(PlayerSpawn playerSpawn)
    {
        playerSpawn.Spawn(hunterSpawnPoint.position);
    }

    private void InitialSpawns()
    {
        foreach(var player in Room.GamePlayers)
        {
            PlayerSpawn playerRespawn = player.GetComponent<PlayerSpawn>();
            if (playerRespawn.isHunter)
                SpawnHunter(playerRespawn);
            else
                SpawnNinja(playerRespawn);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        InitialSpawns();
    }
}
