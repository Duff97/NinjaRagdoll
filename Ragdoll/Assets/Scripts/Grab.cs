using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    private GameObject grabbedObj;
    private FixedJoint joint;

    [Header("References")]
    public Animator animator;


    public bool isGrabbing;


    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrabbing = animator.GetBool("IsGrabbing");
        if (!animator.GetBool("IsGrabbing"))
        {
            ReleaseObj();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator.GetBool("IsGrabbing") && grabbedObj == null)
        {
            GrabObj(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (grabbedObj == other.gameObject)
        {
            ReleaseObj();
        }
    }

    private void GrabObj (GameObject go)
    {
        Rigidbody rigidbody = go.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            grabbedObj = go;
            joint.connectedBody = rigidbody;
        }
    }

    private void ReleaseObj() { 
        if (grabbedObj != null)
        {
            grabbedObj = null;
            joint.connectedBody = null;
        }
    }
}
