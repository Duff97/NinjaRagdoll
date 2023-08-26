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
    public int throwImpulse;
    public float maxGrabVelocity;

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
            CmdReleaseGrabbableAuth(grabbedObj.GetComponent<NetworkIdentity>());
            foreach (FixedJoint j in grabbedObj.GetComponents<FixedJoint>())
            {
                Destroy(j);
            }
            grabbedObj = null;
            targetObj = null;
        }
    }

    private void throwObj()
    {
        if (grabbedObj != null)
        {
            NetworkIdentity grabId = grabbedObj.GetComponent<NetworkIdentity>();
            ReleaseObj();
            CmdThrow(grabId);
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
    private void CmdReleaseGrabbableAuth(NetworkIdentity grabbableId)
    {
        if (grabbableId.connectionToClient == connectionToClient)
        {
            grabbableId.RemoveClientAuthority();
        }
    }

    [Command]
    private void CmdThrow(NetworkIdentity grabbableId)
    {
        if (grabbableId.connectionToClient == connectionToClient)
        {
            grabbableId.RemoveClientAuthority();
            Vector3 throwDirection = throwOrigin.forward;
            grabbableId.GetComponent<Rigidbody>().AddForce(throwDirection * (-throwImpulse), ForceMode.Impulse);
        }
    }
}
