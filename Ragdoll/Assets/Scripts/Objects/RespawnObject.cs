using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    private Vector3 initialPos;
    private NetworkRespawn netRespawn;

    private void Start()
    {
        initialPos = transform.position;
        netRespawn = FindAnyObjectByType<NetworkRespawn>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Respawn"))
        {
            netRespawn.CmdRespawn(transform, initialPos);
        }
        
    }
}
