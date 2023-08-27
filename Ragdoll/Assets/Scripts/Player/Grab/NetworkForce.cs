using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkForce : NetworkBehaviour
{
    [HideInInspector] public NetworkIdentity identity;
    public float forceMultiplicator;
    public float minVelocity;
    private Rigidbody rb;

    private void Start()
    {
        identity = GetComponent<NetworkIdentity>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player") 
            && isServer && connectionToClient == null && rb.velocity.magnitude >= minVelocity)
        {
            VelocityTransfer vtransfer = collision.collider.GetComponent<VelocityTransfer>();
            if (vtransfer != null) 
            {
                // fix this
                //ftransfer.netId.GetComponent<PlayerRespawn>().SetLastAttacker(identity.connectionToClient);
                Vector3 newVelocity = vtransfer.AddVelocity(collision.impulse * -forceMultiplicator);
                RpcApplyVelocity(vtransfer.netId, newVelocity);
            }
        }
        
    }

    [ClientRpc]
    private void RpcApplyVelocity(NetworkIdentity targetId, Vector3 velocity)
    {
        
        VelocityTransfer vtransfer = targetId.GetComponentInChildren<VelocityTransfer>();
        if (vtransfer != null)
        {
            vtransfer.SetVelocity(velocity);
        }
    }
}
