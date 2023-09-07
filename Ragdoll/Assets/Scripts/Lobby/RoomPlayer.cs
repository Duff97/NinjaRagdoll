using Mirror;
using Steamworks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RoomPlayer : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private TMP_Dropdown gameModeDropdown;
    [SerializeField] private Image gameModeImage;
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private TMP_InputField TimeInput;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [HideInInspector][SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    [HideInInspector][SyncVar(hook = nameof(HandleGameTimeChanged))] public int gameTime;
    [HideInInspector][SyncVar(hook = nameof(HandleGameModeChanged))] public int gameMode;

    public static event Action OnLocalPlayerStarted;

    private bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerNinjaRagdoll room;
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(SteamFriends.GetPersonaName());

        lobbyUI.SetActive(true);
    }

    public override void OnStopLocalPlayer()
    {
        if (OnLocalPlayerStarted != null)
            foreach(var client in OnLocalPlayerStarted.GetInvocationList())
            {
                OnLocalPlayerStarted -= client as Action;
            }
        base.OnStopLocalPlayer();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        OnLocalPlayerStarted?.Invoke();
    }

    public override void OnStartClient()
    {
        //TODO FIX THIS
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Room.RoomPlayers.Add(this);

            if (!isLeader)
            {
                TimeInput.enabled = false;
                gameModeDropdown.enabled = false;
            }

            UpdateDisplay();
                
        }
        
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
    public void HandleGameTimeChanged(int oldValue, int newValue) => UpdateDisplay();
    public void HandleGameModeChanged(int oldValue, int newValue) => UpdateDisplay();

    public void QuitLobby()
    {
        if (isLeader)
            Room.StopHost();
        else
            Room.StopClient();
    }

    private void UpdateDisplay()
    {
        if (!isOwned)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isOwned)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }
        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
           
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }

        TimeInput.text = gameTime.ToString();
        gameModeDropdown.value = gameMode;
        gameModeImage.sprite = Room.gameModes[gameMode].sprite;

    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader) { return; }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }

    public void TimeChange()
    {
        int time;
        int.TryParse(TimeInput.text, out time);
        
        if (time > 0)
        {
            CmdTimeChange(time);
        }
    }

    public void GameModeChange()
    {
        CmdGameModeChange(gameModeDropdown.value);
    }

    [Command]
    public void CmdGameModeChange(int gameMode)
    {
        Room.selectedGameMode = gameMode;
        Room.NotifyPlayersOfGameMode();
    }

    [Command]
    public void CmdTimeChange(int time)
    {
        if (time > 0)
        {
            Room.gameTime = time;
            Room.NotifyPlayersOfGameTime();
        }
    }
}
