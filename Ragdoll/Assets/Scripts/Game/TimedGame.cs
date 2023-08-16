using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimedGame : NetworkBehaviour
{
    [SyncVar][SerializeField] private float TimeLeft;

    [SerializeField] TMP_Text timeText;

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
        TimeLeft = Room.GameTime * 60;
    }



    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0)
            {
                //EndGame
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Room.EndGame();
            }
        }
    }

    private void OnGUI()
    {
        if (isClient)
        {
            int minutes = Mathf.FloorToInt(TimeLeft / 60F);
            int seconds = Mathf.FloorToInt(TimeLeft - minutes * 60);

            timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }
}
