using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkVelocity : NetworkBehaviour
{

    public float velocityMultiplicator;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Character") && isOwned && collision.collider.gameObject.name == "Hips")
        {
            Debug.Log("Networked collision enter");
            NetworkIdentity identity = collision.collider.transform.parent.parent.GetComponent<NetworkIdentity>();
            if (!identity.isOwned) 
            {
                Debug.Log("Networked collision detected unowned collision");
                Vector3 targetVelocity = collision.collider.GetComponent<Rigidbody>().velocity;
                CmdApplyVelocity(identity, targetVelocity);
            }
        }
        
    }


    [Command]
    private void CmdApplyVelocity(NetworkIdentity id, Vector3 velocity)
    {
        LimbManager lm = id.GetComponentInChildren<LimbManager>();
        if (lm != null)
        {
            Debug.Log("Velocity applied on server");
            lm.rootBody.velocity = velocity * velocityMultiplicator;
            RpcApplyVelocity(id, velocity);
        }
    }

    [ClientRpc]
    private void RpcApplyVelocity(NetworkIdentity id, Vector3 velocity)
    {
        LimbManager lm = id.GetComponentInChildren<LimbManager>();
        if (lm != null)
        {
            Debug.Log("Velocity applied on client");
            lm.rootBody.velocity = velocity * velocityMultiplicator;
        }
    }
}
