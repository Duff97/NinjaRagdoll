using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkForce : NetworkBehaviour
{
    public NetworkIdentity identity;
    public float forceMultiplicator;

    private void Start()
    {
        identity = GetComponent<NetworkIdentity>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Character") && isOwned)
        {
            ForceDetector detector = collision.collider.GetComponent<ForceDetector>();
            if (detector != null && !detector.netId.isOwned) 
            {
                Debug.Log("Networked collision detected unowned collision");
                CmdApplyForce(detector.netId, collision.impulse, collision.collider.gameObject.name);
            }
        }
        
    }


    [Command]
    private void CmdApplyForce(NetworkIdentity targetId, Vector3 impulse, string limbName)
    {
        LimbManager lm = targetId.GetComponentInChildren<LimbManager>();
        if (lm != null)
        {
            ForceDetector detector = lm.findDetectorByName(limbName);
            targetId.GetComponent<PlayerRespawn>().SetLastAttacker(identity.connectionToClient);
            detector.AddImpulse(impulse * -forceMultiplicator);
            RpcApplyForce(targetId, impulse, limbName);
        }
    }

    [ClientRpc]
    private void RpcApplyForce(NetworkIdentity targetId, Vector3 impulse, string limbName)
    {
        
        LimbManager lm = targetId.GetComponentInChildren<LimbManager>();
        if (lm != null)
        {
            ForceDetector detector = lm.findDetectorByName(limbName);
            if (isOwned)
            {
                detector.forceDetected = (impulse * -forceMultiplicator).magnitude;
            }
            else
            {
                detector.AddImpulse(impulse * -forceMultiplicator);
            } 
        }
    }
}
