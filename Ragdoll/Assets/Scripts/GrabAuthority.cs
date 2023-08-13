using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAuthority : NetworkBehaviour
{
    private NetworkIdentity identity;

    

    private void Awake()
    {
        identity = GetComponent<NetworkIdentity>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Grab grab = other.gameObject.GetComponent<Grab>();
        CmdAssignNetworkAuthority(grab.identity);
    }


    [Command(requiresAuthority=false)]
    public void CmdAssignNetworkAuthority(NetworkIdentity clientId)
    {
        //If -> cube has a owner && owner isn't the actual owner
        if (identity.connectionToClient != null && identity.connectionToClient != clientId.connectionToClient)
        {
            // Remove authority
            identity.RemoveClientAuthority();
        }

        //If -> cube has no owner
        if (identity.connectionToClient == null)
        {
            // Add client as owner
            identity.AssignClientAuthority(clientId.connectionToClient);
        }
    }
}
