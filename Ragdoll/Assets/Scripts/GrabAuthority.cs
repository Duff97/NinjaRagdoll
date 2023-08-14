using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAuthority : NetworkBehaviour
{
    private NetworkIdentity identity;
    private Rigidbody rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        identity = GetComponent<NetworkIdentity>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            Grab grab = other.gameObject.GetComponent<Grab>();
            CmdAssignNetworkAuthority(grab.identity);
        }
    }

    /*public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        rb.isKinematic = false;
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
        rb.isKinematic = true;
    }*/


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
            Debug.Log("Authority assigned to " + clientId.connectionToClient.ToString());
        }
    }

    //TODO Commande qui replique la velocite dune collision sur un autre joueur
}
