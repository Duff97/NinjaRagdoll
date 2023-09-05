using Mirror;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkManagerNinjaRagdoll : NetworkManager
{
    [SerializeField] private int minPlayers;
    [SerializeField] private string menuScene;

    [Header("Room")]
    [SerializeField] private RoomPlayer roomPlayerPrefab;

    [Header("Game")]
    [SerializeField] private Player gamePlayerPrefab;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] public int gameTime;
    [SerializeField] public int selectedGameMode;
    [SerializeField] public List<GameMode> gameModes;

    [HideInInspector] public ulong steamLobbyId;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;

    public List<RoomPlayer> RoomPlayers { get; } = new List<RoomPlayer>();
    public List<Player> GamePlayers { get; } = new List<Player>();

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SteamMatchmaking.LeaveLobby(new CSteamID(steamLobbyId));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

        

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;

            RoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            roomPlayerInstance.gameTime = gameTime;
            roomPlayerInstance.gameMode = selectedGameMode;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<RoomPlayer>();
            
            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();

        NetworkServer.Shutdown();

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    public void NotifyPlayersOfGameTime()
    {
        foreach(var player in RoomPlayers)
        {
            player.gameTime = gameTime;
        }
    }

    public void NotifyPlayersOfGameMode()
    {
        foreach (var player in RoomPlayers)
        {
            player.gameMode = selectedGameMode;
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!IsReadyToStart()) { return; }

            ServerChangeScene(gameModes[selectedGameMode].sceneName);
        }
    }

    public void EndGame()
    {
        
        for (int i = GamePlayers.Count - 1; i >= 0; i--)
        {
            var conn = GamePlayers[i].connectionToClient;
            var roomPlayerInstance = Instantiate(roomPlayerPrefab);
            NetworkServer.Destroy(conn.identity.gameObject);
            NetworkServer.ReplacePlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
        ServerChangeScene(menuScene);

    }

    

    public override void ServerChangeScene(string newSceneName)
    {
        // From menu to game
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Game_"))
        {
            var gameMenuInstance = Instantiate(gameMenu);
            NetworkServer.Spawn(gameMenuInstance.gameObject);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }

    
}
