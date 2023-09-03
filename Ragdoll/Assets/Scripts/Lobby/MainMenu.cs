﻿using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SteamLobby steamLobby;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel;
    [SerializeField] private GameObject playerInputPage;

    public void HostLobby()
    {
        steamLobby.HostLobby();
        HideLandingPage();
    }

    public void Start()
    {
        landingPagePanel.SetActive(PlayerNameInput.DisplayName != "");
        playerInputPage.SetActive(PlayerNameInput.DisplayName == "");
        NetworkManagerNinjaRagdoll.OnClientDisconnected += ReloadScene;
        RoomPlayer.OnLocalPlayerStarted += HideLandingPage;
    }

    private void HideLandingPage()
    {
        landingPagePanel.SetActive(false);
    }


    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void OnDestroy()
    {
        NetworkManagerNinjaRagdoll.OnClientDisconnected -= ReloadScene;
        RoomPlayer.OnLocalPlayerStarted -= HideLandingPage;
    }
}
