using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGrabbable : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleVelocityChanged))][HideInInspector] public Vector3 velocity;
    private Rigidbody rb;

    private void HandleVelocityChanged(Vector3 oldVal, Vector3 newVal)
    {
        rb.velocity = newVal;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
