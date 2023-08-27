using Mirror;
using Mirror.Examples.RigidbodyBenchmark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDetector : MonoBehaviour
{
    public NetworkIdentity netId;
    public float forceDetected = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            forceDetected += collision.impulse.magnitude;
        }
    }

    public void AddImpulse(Vector3 impulse)
    {
        rb.AddForce(impulse, ForceMode.Impulse);
        forceDetected += impulse.magnitude;
    }
}
