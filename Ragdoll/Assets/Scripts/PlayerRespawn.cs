using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    public Transform ragdollPosition;
    public LimbManager limbManagager;
    public float maxDistance;
    

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float distance = (ragdollPosition.position - transform.position).magnitude;
            if (distance > maxDistance)
            {
                ragdollPosition.position = transform.position;
                limbManagager.SetVelocity(Vector3.zero);
            }
        }
    }
}
