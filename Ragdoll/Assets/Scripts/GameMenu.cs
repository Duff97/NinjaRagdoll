using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : NetworkBehaviour
{
    [SerializeField] private KeyCode menuKey = KeyCode.Escape;
    [SerializeField] private GameObject menuPanel;

    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            return NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(menuKey))
        {
            if (menuPanel.activeSelf)
                HideMenu();
            else
                ShowMenu();
            
        }
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        if (isServer)
            Room.StopHost();
        else
            Room.StopClient();
    }
}
