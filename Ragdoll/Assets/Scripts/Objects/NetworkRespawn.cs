using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRespawn : NetworkBehaviour
{
    private GrabbableAttack grabAttack;

    private void Start()
    {
        grabAttack = GetComponent<GrabbableAttack>();
    }

    [Command(requiresAuthority = false)]
    public void CmdRespawn(Transform target, Vector3 initialPosition)
    {
        grabAttack.lastGrabberId = null;
        target.position = initialPosition;
        target.GetComponent<Rigidbody>().velocity = Vector3.zero;
        RpcRespawn(target, initialPosition);
    }

    [ClientRpc]
    private void RpcRespawn(Transform target, Vector3 initialPosition)
    {
        target.position = initialPosition;
        target.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
