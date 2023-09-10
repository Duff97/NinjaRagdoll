using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaSpawn : PlayerSpawn
{
    public LimbManager limbManagager;

    [Server]
    public override void Spawn(Vector3 spawnPosition)
    {
        base.Spawn(spawnPosition);
        limbManagager.SetVelocity(Vector3.zero);
        limbManagager.EnableMovement();
        RpcSpawn(spawnPosition);
    }

    [ClientRpc]
    private void RpcSpawn(Vector3 spawnPosition)
    {
        playerPosition.position = spawnPosition;
        limbManagager.SetVelocity(Vector3.zero);
        limbManagager.EnableMovement();
    }
}
