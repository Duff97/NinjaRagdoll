using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    [Command(requiresAuthority = false)]
    public void CmdShoot(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        rb.velocity = velocity;
    }
}
