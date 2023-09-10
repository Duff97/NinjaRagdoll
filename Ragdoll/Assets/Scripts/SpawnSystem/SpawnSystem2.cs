using Cinemachine.Utility;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem2 : NetworkBehaviour
{
    private Vector3 hunterSpawnPoint;
    private List<Vector3> ninjaSpawnPoints = new List<Vector3>();

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
        playerSpawn.Spawn(ninjaSpawnPoints[index]);
    }

    private void SpawnHunter(PlayerSpawn playerSpawn)
    {
        playerSpawn.Spawn(hunterSpawnPoint);
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
        Transform[] spawnPoints = GetComponentsInChildren<Transform>();
        bool first = true;
        foreach (Transform t in spawnPoints)
        {
            if (t != this.transform)
            {
                if (first)
                {
                    hunterSpawnPoint = t.position;
                    first = false;
                }
                else
                    ninjaSpawnPoints.Add(t.position);
            }
        }
        InitialSpawns();
    }
}
