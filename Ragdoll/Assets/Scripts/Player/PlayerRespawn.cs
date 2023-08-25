using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    [HideInInspector][SyncVar(hook = nameof(HandleSpawnPositionChanged))] public Vector3 spawnPosition;
    public Transform ragdollPosition;
    public LimbManager limbManagager;
    public float maxDistance;
    public NetworkConnectionToClient lastAttacker;
    public int clearAttackerCD;

    public static event Action<NetworkConnectionToClient, NetworkConnectionToClient> OnPlayerRespawn;
    public static event Action<PlayerRespawn> OnServerStarted;
    public static event Action<PlayerRespawn> OnServerStopped;

    public void HandleSpawnPositionChanged(Vector3 oldValue, Vector3 newValue) => TeleportToSpawnPosition();

    public override void OnStartServer()
    {
        base.OnStartServer();
        OnServerStarted?.Invoke(this);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke(this);
        base.OnStopServer();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float distance = ragdollPosition.position.magnitude;
            if (distance > maxDistance)
            {
                TeleportToSpawnPosition();
                CmdRespawnEvent();
            }
        }
    }
    
    public void SetLastAttacker(NetworkConnectionToClient attacker)
    {
        lastAttacker = attacker;
        Invoke(nameof(ClearLastAttacker), clearAttackerCD);

    }

    private void ClearLastAttacker()
    {
        lastAttacker = null;
    }

    [Command]
    private void CmdRespawnEvent()
    {
        OnPlayerRespawn?.Invoke(connectionToClient, lastAttacker);
        lastAttacker = null;
    }

    public void TeleportToSpawnPosition()
    {
        ragdollPosition.position = spawnPosition;
        limbManagager.SetVelocity(Vector3.zero);
    }

    
}
