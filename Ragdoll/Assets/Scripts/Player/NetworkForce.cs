using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkForce : NetworkBehaviour
{

    public float forceMultiplicator;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Character") && isOwned)
        {
            Debug.Log("Networked collision enter");
            ForceDetector detector = collision.collider.GetComponent<ForceDetector>();
            if (detector != null) 
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
            Debug.Log("Force applied on server");
            ForceDetector detector = lm.findDetectorByName(limbName);
            detector.AddImpulse(impulse * forceMultiplicator);
            RpcApplyForce(targetId, impulse, limbName);
        }
    }

    [ClientRpc]
    private void RpcApplyForce(NetworkIdentity targetId, Vector3 impulse, string limbName)
    {
        
        LimbManager lm = targetId.GetComponentInChildren<LimbManager>();
        if (lm != null)
        {
            Debug.Log("Force applied on client");
            ForceDetector detector = lm.findDetectorByName(limbName);
            if (isOwned)
            {
                detector.forceDetected = (impulse * forceMultiplicator).magnitude;
            }
            else
            {
                detector.AddImpulse(impulse * forceMultiplicator);
            }
                
        }
    }
}
