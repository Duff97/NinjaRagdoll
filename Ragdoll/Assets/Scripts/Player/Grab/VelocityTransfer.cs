using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTransfer : MonoBehaviour
{
    public NetworkIdentity netId;
    [SerializeField] private Rigidbody targetRb;
    [SerializeField] private float maxVelocity;

    private LimbManager limbManager;

    private void Start()
    {
        limbManager = GetComponent<LimbManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector3 test = new Vector3();
            test.x = maxVelocity / 3;
            test.y = maxVelocity / 3;
            test.z = maxVelocity / 3;
            SetVelocity(ControlVelocity(test));
        }
    }

    [Server]
    public Vector3 AddVelocity(Vector3 velocity)
    {
        targetRb.velocity = ControlVelocity(targetRb.velocity + velocity);
        limbManager.DisableMovement();
        return targetRb.velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        targetRb.velocity = velocity;
        limbManager.DisableMovement();
    }

    private Vector3 ControlVelocity(Vector3 velocity)
    {
        float y = velocity.y;
        velocity.y = 0;
        velocity += velocity.normalized * y;
        velocity.y = velocity.magnitude;
        if (velocity.magnitude > maxVelocity)
        {
            float factor = maxVelocity/ velocity.magnitude;
            velocity = velocity * factor;
        }
        return velocity;

    }
}
