using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDetector : MonoBehaviour
{
    public float forceDetected = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            forceDetected += collision.impulse.magnitude;
        }
    }
}
