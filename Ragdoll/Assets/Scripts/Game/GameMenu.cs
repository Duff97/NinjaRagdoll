using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenu : NetworkBehaviour
{
    [SerializeField] private GameObject menuPanel;

    public static event Action OnMenuOpened;
    public static event Action OnMenuClosed;

    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            return NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    private void OnOpenMenu()
    {
        if (menuPanel.activeSelf)
            HideMenu();
        else
            ShowMenu();

    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnMenuOpened?.Invoke();
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OnMenuClosed?.Invoke();
    }

    public void QuitGame()
    {
        if (isServer)
            Room.StopHost();
        else
            Room.StopClient();
    }
}
