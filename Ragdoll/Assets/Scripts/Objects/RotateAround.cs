using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : NetworkBehaviour
{
    public Transform pivot;
    public float rotationSpeed;

    public override void OnStartServer()
    {
        base.OnStartServer();
        transform.LookAt(pivot);
    }

    void Update()
    {
        if (!isServer) { return; }

        transform.RotateAround(pivot.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
