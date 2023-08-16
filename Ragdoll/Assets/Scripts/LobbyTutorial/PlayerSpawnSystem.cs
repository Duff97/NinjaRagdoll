using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class PlayerSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private Vector3 spawnPosition = Vector3.zero;

        public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;

        [ServerCallback]
        private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            GameObject playerInstance = Instantiate(playerPrefab);
            playerInstance.transform.position = spawnPosition;
            NetworkServer.Spawn(playerInstance, conn);
        }
    }
}
