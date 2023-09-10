using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSpawn : PlayerSpawn
{
    public Rigidbody rb;

    [Server]
    public override void Spawn(Vector3 spawnPosition)
    {
        base.Spawn(spawnPosition);
        rb.velocity = Vector3.zero;
        RpcSpawn(spawnPosition);
    }

    [ClientRpc]
    private void RpcSpawn(Vector3 spawnPosition)
    {
        playerPosition.position = spawnPosition;
        rb.velocity = Vector3.zero;
    }
}
