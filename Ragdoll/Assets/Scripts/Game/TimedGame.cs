using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimedGame : NetworkBehaviour
{
    [SyncVar][SerializeField] private float timeLeft;
    private bool gameEnded = false;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject scoreBoardObj;
    [SerializeField] GameObject endgameObj;
    


    private NetworkManagerNinjaRagdoll room;
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        timeLeft = Room.gameTime * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer && !gameEnded)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                RpcEndGame();
                gameEnded = true;
                Room.EndGame();
            }
        }
    }

    private void OnGUI()
    {
        if (isClient)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60F);
            int seconds = Mathf.FloorToInt(timeLeft - minutes * 60);

            timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        scoreBoardObj.SetActive(true);
        endgameObj.SetActive(true);
    }
}
