using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkPlayerSettings : NetworkBehaviour
{
    public CinemachineFreeLook targetCamera;

    private const string PPCameraSpeedX = "CameraSpeedX";
    private const string PPCameraSpeedY = "CameraSpeedY";

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        LoadFromPrefs();
    }

    private void LoadFromPrefs()
    {
        if (PlayerPrefs.HasKey(PPCameraSpeedX))
            targetCamera.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat(PPCameraSpeedX);

        if (PlayerPrefs.HasKey(PPCameraSpeedY))
            targetCamera.m_YAxis.m_MaxSpeed = PlayerPrefs.GetFloat(PPCameraSpeedY);
    }
}
