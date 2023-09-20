using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : NetworkBehaviour
{
    private bool gameEnded;
    [SerializeField][SyncVar(hook = nameof(HandleTimeChanged))] float timeUntilGameCloses;
    [SyncVar(hook = nameof(HandleOutcomeChanged))] private string outcome;

    [Header("Client")]
    [SerializeField] private TMP_Text timeUntilGameClosesText;
    [SerializeField] private TMP_Text outcomeText;
    [SerializeField] private Button quitToLobbyBtn;
    [SerializeField] private GameObject endGamePanel;

    public static event Action OnGameEnded;

    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            return NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isServer)
            quitToLobbyBtn.interactable = false;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        LootCounter.OnLootLimit += HandleLootCounterFinished;
        TimeCounter.OnTimeCounterFinished += HandleTimeCounterFinished;


    }

    public override void OnStopServer()
    {
        LootCounter.OnLootLimit -= HandleLootCounterFinished;
        TimeCounter.OnTimeCounterFinished -= HandleTimeCounterFinished;
        base.OnStopServer();
    }

    private void Update()
    {
        if (!gameEnded || !isServer) { return; }

        timeUntilGameCloses -= Time.deltaTime;

        if (timeUntilGameCloses <= 0)
        {
            gameEnded = false;
            QuitToLobby();
        }

        
    }

    public void QuitToMainMenu()
    {
        if (isServer)
            Room.StopHost();
        else
            Room.StopClient();
    }

    [Server]
    public void QuitToLobby()
    {
        Room.EndGame();
    }

    [Server]
    private void HandleTimeCounterFinished()
    {
        outcome = "The Hunter Wins!";
        InitializeEndGame();
    }

    [Server]
    private void HandleLootCounterFinished()
    {
        outcome = "The Ninjas Wins!";
        InitializeEndGame();
    }

    private void InitializeEndGame()
    {
        RpcEndGame();
        gameEnded = true;
        OnGameEnded?.Invoke();
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        endGamePanel.SetActive(true);
    }

    [Client]
    private void HandleOutcomeChanged(string oldVal, string newVal)
    {
        outcomeText.text = newVal;
    }

    [Client]
    private void HandleTimeChanged(float oldVal, float newVal)
    {
        timeUntilGameClosesText.text = Mathf.Ceil(newVal) + " seconds until game closes";
    }



}
