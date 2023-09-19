using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPortal : NetworkBehaviour
{
    public static event Action<int> OnLootReceived;
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

        OnLootReceived?.Invoke(loot.value);
        NetworkServer.Destroy(loot.gameObject);
    }
}
