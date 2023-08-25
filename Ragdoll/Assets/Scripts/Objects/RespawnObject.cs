using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : NetworkBehaviour
{
    private Vector3 initialPos;
    [SerializeField] private float maxDistance;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        initialPos = transform.position;
        
    }

    private void Update()
    {
        if (isServer)
        {
            float distance = (transform.position - initialPos).magnitude;
            if (distance > maxDistance)
            {
                Respawn();
            }
        }
    }

    [Server]
    private void Respawn()
    {
        transform.position = initialPos;
        rb.velocity = Vector3.zero;
        RpcRespawn(initialPos);

    }

    [ClientRpc]
    private void RpcRespawn(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero;
    }
}
