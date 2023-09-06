using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : NetworkBehaviour
{
    [SyncVar] public bool gameEnded;
    [SerializeField][SyncVar] float timeUntilGameCloses;
    [SerializeField] private TMP_Text timeUntilGameClosesText;
    [SerializeField] private TMP_Text outcomeText;
    [SerializeField] private Button quitToLobbyBtn;
    private ScoreBoard scoreBoard;

    private bool returningToLobby = false;

    private NetworkManagerNinjaRagdoll room;
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    private void Awake()
    {
        scoreBoard = GetComponent<ScoreBoard>();
    }

    private void Start()
    {
        if (!isServer)
        {
            quitToLobbyBtn.interactable = false;
        }
    }

    private void Update()
    {
        if (gameEnded && !returningToLobby)
        {
            if (isServer)
            {
                timeUntilGameCloses -= Time.deltaTime;
                if (timeUntilGameCloses <= 0)
                {
                    returningToLobby = true;
                    QuitToLobby();
                }
            }

            if (isClient)
            {
                timeUntilGameClosesText.text = Mathf.Ceil(timeUntilGameCloses).ToString() + " seconds until game closes";
                outcomeText.text = scoreBoard.GetWinnerName() + " wins!";
            }
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

}
