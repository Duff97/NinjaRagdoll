using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    public Transform ragdollPosition;
    public LimbManager limbManagager;
    public float maxDistance;
    public NetworkConnectionToClient lastAttacker;
    public int clearAttackerCD = 4;

    public static event Action<NetworkConnectionToClient, NetworkConnectionToClient> OnPlayerRespawn;

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float distance = (ragdollPosition.position - transform.position).magnitude;
            if (distance > maxDistance)
            {
                ragdollPosition.position = transform.position;
                limbManagager.SetVelocity(Vector3.zero);
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
        Debug.Log("OnPLayerRespawn event fired");
        OnPlayerRespawn?.Invoke(connectionToClient, lastAttacker);
        lastAttacker = null;
    }

    
}
