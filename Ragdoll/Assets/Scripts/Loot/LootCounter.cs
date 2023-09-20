using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootCounter : NetworkBehaviour
{
    public TMP_Text lootText;
    [SyncVar(hook = nameof(HandleLootValueChanged))] private int lootValue;
    private const int lootTreshold = 100;

    public static event Action OnLootLimit;


    public override void OnStartServer()
    {
        base.OnStartServer();
        LootPortal.OnLootReceived += AddLoot;
        lootValue = 0;
    }

    public override void OnStopServer()
    {
        LootPortal.OnLootReceived -= AddLoot;
        base.OnStopServer();
    }

    [Client]
    private void HandleLootValueChanged(int oldVal, int newVal)
    {
        lootText.text = "Loot: " + newVal.ToString() + "%";
    }

    [Server]
    private void AddLoot(int value)
    {
        lootValue += value;

        if (lootValue >= lootTreshold)
            OnLootLimit?.Invoke();

    }


}
