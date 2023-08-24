using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    [HideInInspector] public Transform spawnPosition;
    public Transform ragdollPosition;
    public LimbManager limbManagager;
    public float maxDistance;
    public NetworkConnectionToClient lastAttacker;
    public int clearAttackerCD;

    public static event Action<NetworkConnectionToClient, NetworkConnectionToClient> OnPlayerRespawn;
    public static event Action<PlayerRespawn> OnServerStarted;
    public static event Action<PlayerRespawn> OnServerStopped;

    public override void OnStartServer()
    {
        Debug.Log("On start server on player respawn");
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
                Teleport(spawnPosition.position);
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

    public void Teleport(Vector3 pos)
    {
        ragdollPosition.position = pos;
        limbManagager.SetVelocity(Vector3.zero);
    }

    
}
