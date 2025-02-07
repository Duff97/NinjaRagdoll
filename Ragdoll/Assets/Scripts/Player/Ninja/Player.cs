using Cinemachine;
using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SyncVar]
    public string displayName = "Loading...";

    private NetworkManagerNinjaRagdoll room;
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        if (Room != null)
            Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        if (Room != null)
            Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
