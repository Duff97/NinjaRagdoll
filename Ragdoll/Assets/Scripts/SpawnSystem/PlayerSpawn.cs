using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    public Transform playerPosition;
    public float waterLineHeight;
    public bool isHunter;

    public static event Action<PlayerSpawn> OnSpawnRequest;

    [Server]
    public virtual void Spawn(Vector3 spawnPosition)
    {
        playerPosition.position = spawnPosition;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }

        if (playerPosition.position.y >= waterLineHeight) { return; }

        CmdSpawnRequest();
        
    }

    [Command]
    private void CmdSpawnRequest()
    {
        OnSpawnRequest?.Invoke(this);
    }

}
