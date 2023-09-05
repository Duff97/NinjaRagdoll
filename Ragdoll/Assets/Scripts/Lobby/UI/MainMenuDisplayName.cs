using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuDisplayName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayNameText;

    private void Start()
    {
        if (!SteamManager.Initialized) { return; }

        displayNameText.text = "Playing as " + SteamFriends.GetPersonaName();
    }
}
