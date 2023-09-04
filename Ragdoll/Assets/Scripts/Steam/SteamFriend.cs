using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SteamFriend : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private string steamAppId;
    public void DisplaySteamInfo(CSteamID steamID)
    {
        nameText.text = SteamFriends.GetFriendPersonaName(steamID);
        statusText.text = "Online";

        FriendGameInfo_t friendGameInfo;
        
        if (SteamFriends.GetFriendGamePlayed(steamID, out friendGameInfo))
        {
            if (friendGameInfo.m_gameID.ToString().Equals(steamAppId))
            {
                transform.SetSiblingIndex(0);
                statusText.text = "In game";
            }
        }

    }
}
