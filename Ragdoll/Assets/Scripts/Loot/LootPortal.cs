using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPortal : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer || other.tag != "Loot") { return; }

        Loot loot = other.GetComponent<Loot>();
        collectLoot(loot);
    }

    [Server]
    private void collectLoot(Loot loot)
    {
        if (loot == null) return;

        NetworkServer.Destroy(loot.gameObject);
    }
}
