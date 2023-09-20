using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCounter : NetworkBehaviour
{
    [SerializeField][SyncVar(hook = nameof(HandleTimeChange))] private float timeLeft;
    [SerializeField] TMP_Text timeText;

    public static event Action OnTimeCounterFinished;
    
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            return NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        timeLeft = Room.gameTime * 60;
        //timeLeft = 5;
        
    }

    private void Update()
    {
        if (!isServer || timeLeft <= 0) { return; }

        float timeSpent = Time.deltaTime;

        if (timeSpent > timeLeft)
            timeLeft = 0;
        else
            timeLeft -= timeSpent;

        if (timeLeft <= 0)
            OnTimeCounterFinished?.Invoke();
    }

    [Client]
    private void HandleTimeChange(float oldVal, float newVal)
    {
        int minutes = Mathf.FloorToInt(newVal / 60F);
        int seconds = Mathf.FloorToInt(newVal - minutes * 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
