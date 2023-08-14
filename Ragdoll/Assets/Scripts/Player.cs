using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private SpineControl spineControl;

    public override void OnStartAuthority()
    {
        cameraHolder.GetComponent<CinemachineFreeLook>().Priority = 10;
        cameraHolder.GetComponent<ThirdPersonCam>().hasAuthority = true;
        spineControl.hasAuthority = true;
        
    }

    [Command]
    public void CmdAddForceToBody(Vector3 force, NetworkIdentity identity)
    {
        Debug.Log("Applied force command");
        Rigidbody rb = identity.GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.Impulse);
        CRpcAddForceToBody(force, identity);
    }

    [ClientRpc]
    public void CRpcAddForceToBody(Vector3 force, NetworkIdentity identity)
    {
        Debug.Log("Applied force rpc");
        Rigidbody rb = identity.GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.Impulse);
    }
}
