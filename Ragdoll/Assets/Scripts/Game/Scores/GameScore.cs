using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : NetworkBehaviour
{
    public static event Action<NetworkConnectionToClient, int> OnPointGained;

    [Server]
    protected void GivePoint(NetworkConnectionToClient playerId, int points)
    {
        OnPointGained?.Invoke(playerId, points);
    }


}
