using Cinemachine;
using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private SpineControl spineControl;

    public override void OnStartAuthority()
    {
        cameraHolder.GetComponent<CinemachineFreeLook>().Priority = 10;
        cameraHolder.GetComponent<ThirdPersonCam>().hasAuthority = true;
        spineControl.hasAuthority = true;
    }

    [SyncVar]
    private string displayName = "Loading...";

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}