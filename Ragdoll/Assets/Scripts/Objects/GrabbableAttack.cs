using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableAttack : NetworkBehaviour
{
    [SyncVar] public NetworkIdentity lastGrabberId;

    private void Update()
    {
        if (isServer)
        {
            if (connectionToClient != null && (lastGrabberId == null || lastGrabberId.connectionToClient != connectionToClient))
                lastGrabberId = connectionToClient.identity;
        }
    }

    [Server]
    public void SetPlayerLastAttacker(PlayerRespawn playerRespawn)
    {
        if (lastGrabberId != null)
            playerRespawn.SetLastAttacker(lastGrabberId.connectionToClient);
    }
}
