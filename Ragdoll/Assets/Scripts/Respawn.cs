using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : NetworkBehaviour
{
    private Vector3 initialPos;
    private Rigidbody rb;

    private void Start()
    {
        initialPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Respawn"))
        {
            Debug.Log("Respawn object replaced");
            transform.position = initialPos;
            rb.velocity = Vector3.zero;
        }
        
    }
}
