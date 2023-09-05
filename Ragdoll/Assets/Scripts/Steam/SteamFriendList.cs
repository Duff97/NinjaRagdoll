using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamFriendList : MonoBehaviour
{
    [SerializeField] private GameObject scrollableContent;
    [SerializeField] private SteamFriend steamFriendPrefab;
    [SerializeField] private float refreshInterval;
    private float currentRefreshTime;

    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized) { return; }

        RefreshFriends();

    }

    private void Update()
    {
        if (!SteamManager.Initialized) { return; }

        currentRefreshTime -= Time.deltaTime;

        if (currentRefreshTime <= 0)
            RefreshFriends();
    }

    private void ClearFriends()
    {
        foreach (Transform child in scrollableContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshFriends()
    {
        currentRefreshTime = refreshInterval;
        ClearFriends();
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
