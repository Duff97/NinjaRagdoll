using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamFriendList : MonoBehaviour
{
    [SerializeField] private GameObject scrollableContent;
    [SerializeField] private SteamFriend steamFriendPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized) { return; }

        Debug.Log("Start");
        
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        for (int i = 0; i < friendCount; i++)
        {
            CSteamID steamId = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            if (!(SteamFriends.GetFriendPersonaState(steamId) == EPersonaState.k_EPersonaStateOffline ||
                SteamFriends.GetFriendPersonaState(steamId) == EPersonaState.k_EPersonaStateInvisible))
            {
                SteamFriend friend = Instantiate(steamFriendPrefab);
                friend.transform.SetParent(scrollableContent.transform, false);
                friend.DisplaySteamInfo(steamId);
            }
        }

    }
}
