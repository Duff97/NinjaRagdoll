using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputToggle : NetworkBehaviour
{
    public PlayerInput playerInput;
    public ThirdPersonCam playerCamera;
    public override void OnStartLocalPlayer()
    {
        base.OnStopLocalPlayer();
        GameMenu.OnMenuOpened += DisableInputs;
        GameMenu.OnMenuClosed += EnableInputs;
    }

    public override void OnStopLocalPlayer()
    {
        GameMenu.OnMenuOpened -= DisableInputs;
        GameMenu.OnMenuClosed -= EnableInputs;
        base.OnStopLocalPlayer();
    }

    private void EnableInputs()
    {
        playerInput.ActivateInput();
        playerCamera.EnableCameraControl();
    }

    private void DisableInputs() 
    {
        playerInput.DeactivateInput();
        playerCamera.DisableCameraControl();
    }
}
