using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;

    public override void OnStartServer() => NetworkManagerNinjaRagdoll.OnServerReadied += SpawnPlayer;

    [ServerCallback]
    private void OnDestroy() => NetworkManagerNinjaRagdoll.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        //GameObject playerInstance = conn.identity.gameObject;
        //playerInstance.transform.position = spawnPosition;
    }
}
