using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{

    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private SpineControl spineControl;

    public override void OnStartAuthority()
    {
        cameraHolder.GetComponent<CinemachineFreeLook>().Priority = 10;
        cameraHolder.GetComponent<ThirdPersonCam>().hasAuthority = true;
        spineControl.hasAuthority = true;
        
    }
}
