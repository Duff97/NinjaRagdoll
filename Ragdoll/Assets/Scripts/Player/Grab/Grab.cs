using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class Grab : NetworkBehaviour
{
    public GameObject grabbedObj;
    private GameObject targetObj;

    [Header("Hands")]
    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform throwOrigin;
    [SerializeField] private LimbManager limbManager;

    [Header("Parameters")]
    [SerializeField] private int throwImpulse;
    [SerializeField] private float maxGrabVelocity;

    [Header("Inputs")]
    public KeyCode throwInput = KeyCode.Mouse1;


    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();
        leftHand.OnTriggerEntered += HandleTriggerEntered;
        rightHand.OnTriggerEntered += HandleTriggerEntered;

    }

    public override void OnStopClient()
    {
        leftHand.OnTriggerEntered -= HandleTriggerEntered;
        rightHand.OnTriggerEntered -= HandleTriggerEntered;
        base.OnStopClient();
    }

    private void HandleTriggerEntered(Collider other)
    {
        if (animator.GetBool("IsGrabbing") && grabbedObj == null && other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            targetObj = other.gameObject;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (!animator.GetBool("IsGrabbing"))
            {
                if (targetObj != null)
                    targetObj = null;
            }

            if (targetObj != grabbedObj)
            {
                ReleaseObj();
                GrabObj(targetObj);
            }

            if (Input.GetKey(throwInput))
                throwObj();
        }
    }

    private void GrabObj(GameObject go)
    {
        if (go != null)
        {
            CmdTakeGrabbableAuth(go.GetComponent<NetworkIdentity>());
            grabbedObj = go;
            FixedJoint joint1 = go.AddComponent<FixedJoint>();
            joint1.connectedBody = leftHand.rb;
            FixedJoint joint2 = go.AddComponent<FixedJoint>();
            joint2.connectedBody = rightHand.rb;
        }
    }

    private void ReleaseObj() {
        if (grabbedObj != null)
        {
            RemoveJoints();
            Rigidbody grabbedRb = grabbedObj.GetComponent<Rigidbody>();
            CmdReleaseGrabbableAuth(grabbedObj.GetComponent<NetworkIdentity>(), grabbedRb.velocity);
            grabbedObj = null;
            targetObj = null;
        }
    }

    private void throwObj()
    {
        if (grabbedObj != null)
        {
            RemoveJoints();
            Rigidbody grabbedRb = grabbedObj.GetComponent<Rigidbody>();
            CmdThrow(grabbedObj.GetComponent<NetworkIdentity>(), grabbedRb.velocity);
            grabbedObj = null;
            targetObj = null;
        }
    }

    private void RemoveJoints()
    {
        foreach (FixedJoint j in grabbedObj.GetComponents<FixedJoint>())
        {
            Destroy(j);
        }
    }

    [Command]
    private void CmdTakeGrabbableAuth(NetworkIdentity grabbableId)
    {
        //If -> cube has a owner && owner isn't the actual owner
        if (grabbableId.connectionToClient != null && grabbableId.connectionToClient != connectionToClient)
        {
            // Remove authority
            grabbableId.RemoveClientAuthority();
        }

        //If -> cube has no owner
        if (grabbableId.connectionToClient == null)
        {
            // Add client as owner
            grabbableId.AssignClientAuthority(connectionToClient);
        }
    }

    [Command]
    private void CmdReleaseGrabbableAuth(NetworkIdentity grabbableId, Vector3 velocity)
    {
        if (grabbableId.connectionToClient == connectionToClient)
        {
            grabbableId.RemoveClientAuthority();
            NetworkGrabbable netGrab = grabbableId.GetComponent<NetworkGrabbable>();
            netGrab.velocity = velocity;
        }
    }

    [Command]
    private void CmdThrow(NetworkIdentity grabbableId, Vector3 velocity)
    {
        if (grabbableId.connectionToClient == connectionToClient)
        {
            grabbableId.RemoveClientAuthority();
            NetworkGrabbable netGrab = grabbableId.GetComponent<NetworkGrabbable>();
            Rigidbody grabRb = grabbableId.GetComponent<Rigidbody>();
            netGrab.velocity = velocity + ThrowVelocity(grabRb.mass);
        }
    }

    [Server]
    private Vector3 ThrowVelocity(float mass)
    {
        Vector3 throwDirection = throwOrigin.forward;
        Vector3 force = throwDirection * (-throwImpulse);
        return (1 / mass) * force;
    }
}
