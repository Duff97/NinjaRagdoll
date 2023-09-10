using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    public Transform playerPosition;

    [Server]
    public virtual void Spawn(Vector3 spawnPosition)
    {
        playerPosition.position = spawnPosition;
    }

}
