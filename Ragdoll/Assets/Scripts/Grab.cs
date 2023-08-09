using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    private GameObject grabbedObj;
    private GameObject targetObj;
    private Rigidbody rb;

    [Header("References")]
    public Animator animator;

    [Header("Parameters")]
    public int breakForce = 9000;
    public float massScale = 0.1f;

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
            targetObj = null;
        }

        if (targetObj != grabbedObj)
        {
            ReleaseObj();
            GrabObj(targetObj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator.GetBool("IsGrabbing") && grabbedObj == null)
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
        if (go != null && go.GetComponent<Rigidbody>() != null)
        {
            grabbedObj = go;
            joint = go.AddComponent<FixedJoint>();
            joint.connectedBody = rb;
            joint.breakForce = breakForce;
            joint.connectedMassScale = massScale;
        }
    }

    private void ReleaseObj() { 
        if (grabbedObj != null)
        {
            Destroy(joint);
            grabbedObj = null;
        }
    }
}
