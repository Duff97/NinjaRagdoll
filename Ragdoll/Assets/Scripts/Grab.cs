using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public GameObject grabbedObj;
    private GameObject targetObj;
    private Rigidbody rb;
    private SphereCollider trigger;

    [Header("References")]
    public Animator animator;
    public Transform throwOrigin;
    public NetworkIdentity identity;
    public LimbManager limbManager;
    private Player player;

    [Header("Parameters")]
    public int breakForce;
    public int massScale;
    public int throwImpulse;
    public float throwCD;

    [Header("Inputs")]
    public KeyCode throwInput = KeyCode.Mouse1;

    private FixedJoint joint;
    bool grabDisabled;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trigger = GetComponent<SphereCollider>();
        player = identity.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        trigger.enabled = animator.GetBool("IsGrabbing") && !grabDisabled;
        if (!animator.GetBool("IsGrabbing"))
        {
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

    private void OnTriggerEnter(Collider other)
    {
        if (animator.GetBool("IsGrabbing") && grabbedObj == null && other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            targetObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (grabbedObj == other.gameObject)
        {
            targetObj = null;
        }
    }

    private void GrabObj (GameObject go)
    {
        if (go != null)
        {
            grabbedObj = go;
            joint = go.AddComponent<FixedJoint>();
            joint.connectedBody = rb;
            joint.massScale = massScale;
            limbManager.IgnoreCollision(grabbedObj.GetComponent<Collider>(), true);

        }
    }

    private void ReleaseObj() { 
        if (grabbedObj != null)
        {
            limbManager.IgnoreCollision(grabbedObj.GetComponent<Collider>(), false);
            Destroy(joint);
            grabbedObj = null;
            targetObj = null;
        }
    }

    private void throwObj()
    {
        if (grabbedObj != null)
        {
            Vector3 throwDirection = throwOrigin.forward;
            grabbedObj.GetComponent<Rigidbody>().AddForce(throwDirection * (-throwImpulse), ForceMode.Impulse);
            //player.CmdAddForceToBody(throwDirection * (-throwImpulse), grabbedObj.GetComponent<NetworkIdentity>());
            grabDisabled = true;
            Invoke("ActivateGrab", throwCD);
            ReleaseObj();
        }    
    }

    private void ActivateGrab()
    {
        grabDisabled = false;
    }
}
