using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGameScore : GameScore
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        PointZone.OnPointZoneTriggered += HandlePoints;
    }

    public override void OnStopServer()
    {
        PointZone.OnPointZoneTriggered -= HandlePoints;
        base.OnStopServer();
    }

    private void HandlePoints(NetworkConnectionToClient playerConn, int points)
    {
        this.GivePoint(playerConn, points);
    }
}
