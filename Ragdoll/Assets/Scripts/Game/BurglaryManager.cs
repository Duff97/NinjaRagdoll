using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BurglaryManager : NetworkBehaviour
{
    [SyncVar][SerializeField] private float timeLeft;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject endgameObj;
    [SerializeField] LootCounter lootCounter;
    [SerializeField] EndGame endGame;
    
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            return NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        endgameObj.SetActive(true);
        timeText.gameObject.SetActive(false);
    }
}
