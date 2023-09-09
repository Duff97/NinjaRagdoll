using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : NetworkBehaviour
{
    [SerializeField] private GameObject cameraHolder;

    public override void OnStartAuthority()
    {
        cameraHolder.GetComponent<CinemachineFreeLook>().Priority = 10;
        cameraHolder.GetComponent<HunterCam>().hasAuthority = true;
    }
}
