using Cinemachine.Utility;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem2 : NetworkBehaviour
{
    public Transform hunterSpawnPoint;
    public List<Transform> ninjaSpawnPoints;

    private void SpawnNinja(PlayerSpawn playerSpawn)
    {
        int index = Random.Range(0, ninjaSpawnPoints.Count);
        playerSpawn.Spawn(ninjaSpawnPoints[index].position);
    }

    private void SpawnHunter(PlayerSpawn playerSpawn)
    {
        playerSpawn.Spawn(hunterSpawnPoint.position);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        PlayerSpawn.OnSpawnRequest += HandleSpawnRequest;
    }

    private void HandleSpawnRequest(PlayerSpawn playerSpawn)
    {
        if (playerSpawn.isHunter)
            SpawnHunter(playerSpawn);
        else
            SpawnNinja(playerSpawn);
    }

    public override void OnStopServer()
    {
        PlayerSpawn.OnSpawnRequest -= HandleSpawnRequest;
        base.OnStopServer();
    }
}
