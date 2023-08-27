using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTransfer : MonoBehaviour
{
    public NetworkIdentity netId;
    [SerializeField] private Rigidbody targetRb;
    [SerializeField] private float upwardForce;

    private LimbManager limbManager;

    private void Start()
    {
        limbManager = GetComponent<LimbManager>();
    }

    [Server]
    public Vector3 AddVelocity(Vector3 velocity)
    {
        //targetRb.AddForce(impulse, ForceMode.Impulse);
        velocity.y += upwardForce;
        targetRb.velocity += velocity;
        limbManager.DisableMovement();
        return targetRb.velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        targetRb.velocity = velocity;
        limbManager.DisableMovement();
    }
}
