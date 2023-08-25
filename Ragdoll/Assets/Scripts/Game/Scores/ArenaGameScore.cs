using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGameScore : GameScore
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        PlayerRespawn.OnPlayerRespawn += HandlePoints;
    }

    public override void OnStopServer()
    {
        PlayerRespawn.OnPlayerRespawn -= HandlePoints;
        base.OnStopServer();
    }

    private void HandlePoints(NetworkConnectionToClient victimConn, NetworkConnectionToClient attackerConn)
    {
        if (attackerConn == null)
            this.GivePoint(victimConn, -1);
        else
            this.GivePoint(attackerConn, 1);
    }
}
