using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRespawn : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public void CmdRespawn(Transform target, Vector3 initialPosition)
    {
        target.position = initialPosition;
        Rigidbody rb = target.GetComponent<Rigidbody>();
        RpcRespawn(target, initialPosition);
    }

    [ClientRpc]
    private void RpcRespawn(Transform target, Vector3 initialPosition)
    {
        target.position = initialPosition;
        target.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
