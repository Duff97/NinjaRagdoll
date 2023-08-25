using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointZone : NetworkBehaviour
{
    private List<NetworkIdentity> ids = new List<NetworkIdentity>();
    [SerializeField] private float timeBetweenTriggers;
    [SerializeField] private int point;

    private float timeUntilTrigger;

    public static event Action<NetworkConnectionToClient, int> OnPointZoneTriggered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player enters point zone");
        if (isServer && other.tag == "Player")
        {
            Debug.Log("Player id added to list");
            ids.Add(other.GetComponent<ForceDetector>().netId);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isServer)
        {
            ids.Remove(other.GetComponent<ForceDetector>().netId);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        timeUntilTrigger = timeBetweenTriggers;
    }

    private void Update()
    {
        if (isServer)
        {
            timeUntilTrigger -= Time.deltaTime;
            if (timeUntilTrigger <= 0)
            {
                foreach (NetworkIdentity id in ids)
                {
                    OnPointZoneTriggered?.Invoke(id.connectionToClient, point);
                }
                timeUntilTrigger = timeBetweenTriggers;
            }
        }
    }


}
