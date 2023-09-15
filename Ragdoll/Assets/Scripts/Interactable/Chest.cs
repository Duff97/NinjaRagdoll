using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : NetworkBehaviour
{
    public Transform lootSpawnPoint;
    public float openingEffort;
    public GameObject lootPrefab;
    public Collider openingZone;

    private float currentEffortDone;
    [SyncVar] public bool isInteractable;

    public override void OnStartServer()
    {
        base.OnStartServer();
        isInteractable = true;
        currentEffortDone = 0;
    }

    [Command(requiresAuthority = false)]
    public void addOpenEffort(float effort)
    {
        if (!isInteractable) { return; }

        currentEffortDone += effort;

        if (currentEffortDone >= openingEffort)
            Open();
    }

    [Server]
    private void Open()
    {
        if (!isInteractable) { return;}

        isInteractable = false;
        GameObject loot = Instantiate(lootPrefab);
        loot.transform.position = lootSpawnPoint.position;
        NetworkServer.Spawn(loot);
        openingZone.enabled = false;
    }
}
