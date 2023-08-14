using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public GameObject grabbedObj;
    private GameObject targetObj;
    private Rigidbody rb;

    [Header("References")]
    public Animator animator;
    public Transform throwOrigin;
    public NetworkIdentity identity;
    public LimbManager limbManager;

    [Header("Parameters")]
    public int throwImpulse;
    public float maxGrabVelocity;

    [Header("Inputs")]
    public KeyCode throwInput = KeyCode.Mouse1;

    private FixedJoint joint;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("IsGrabbing"))
        {
            if (targetObj!= null) 
                Debug.Log("Object released because animation stop");
            targetObj = null;
        }

        if (targetObj != grabbedObj)
        {
            Debug.Log("Object released because targetObj != grabbedObj");
            ReleaseObj();
            GrabObj(targetObj);
        }

        if (Input.GetKey(throwInput))
            throwObj();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator.GetBool("IsGrabbing") && grabbedObj == null && other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            targetObj = other.gameObject;
        }
    }

    private void GrabObj (GameObject go)
    {
        if (go != null)
        {
            GrabAuthority grabAuthority = go.GetComponent<GrabAuthority>();
            if (!grabAuthority.grabDisabled)
            {
                Debug.Log("Object grabbed");
                grabbedObj = go;
                joint = go.AddComponent<FixedJoint>();
                joint.connectedBody = rb;
            }

        }
    }

    private void ReleaseObj() { 
        if (grabbedObj != null)
        {
            Debug.Log("Object released function");
            Destroy(joint);
            grabbedObj = null;
            targetObj = null;
        }
    }

    private void throwObj()
    {
        if (grabbedObj != null)
        {
            grabbedObj.GetComponent<GrabAuthority>().CmdStartGrabCD();
            Vector3 throwDirection = throwOrigin.forward;
            grabbedObj.GetComponent<Rigidbody>().AddForce(throwDirection * (-throwImpulse), ForceMode.Impulse);
            ReleaseObj();
        }    
    }
}
