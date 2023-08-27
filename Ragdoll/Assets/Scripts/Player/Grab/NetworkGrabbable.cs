using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NetworkGrabbable : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleVelocityChanged))][HideInInspector] public Vector3 velocity;
    [SerializeField] private float syncDistance = 5;
    [SerializeField] private float positionSyncInterval = 2;
    private float timeUntilSync;
    private Rigidbody rb;

    private void HandleVelocityChanged(Vector3 oldVal, Vector3 newVal)
    {
        rb.velocity = newVal;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        timeUntilSync = positionSyncInterval;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isServer && connectionToClient == null)
        {
            timeUntilSync -= Time.deltaTime;
            if (timeUntilSync <= 0)
            {
                timeUntilSync = positionSyncInterval;
                RpcSetPosition(rb.position, rb.velocity);
            }
        }
    }

    [ClientRpc]
    private void RpcSetPosition(Vector3 position, Vector3 velocity)
    {
        if ((rb.position - position).magnitude >= syncDistance)
        {
            rb.position = position;
            rb.velocity = velocity;
        }
    }
}
