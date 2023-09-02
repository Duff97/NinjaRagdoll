using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Slider CameraSpeedXSlider;
    public Slider CameraSpeedYSlider;
    public Slider VolumeSlider;
    public Toggle FullScreenToggle;
    private Resolution[] resolutions;

    private const string PPCamSpeedX = "CameraSpeedX";
    private const string PPCamSpeedY = "CameraSpeedY";
    private const string PPFullScreen = "FullScreen";
    private const string PPResolution = "Resolution";
    private const string PPVolume = "Volume";

    public static event Action OnSettingsChanged;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        LoadFromPrefs();

    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetFullScreen(bool isFullSreen)
    {
        Screen.fullScreen = isFullSreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    public void Confirm()
    {
        PlayerPrefs.SetFloat(PPCamSpeedX, CameraSpeedXSlider.value);
        PlayerPrefs.SetFloat(PPCamSpeedY, CameraSpeedYSlider.value);
        PlayerPrefs.SetFloat(PPVolume, VolumeSlider.value);
        PlayerPrefs.SetInt(PPFullScreen, FullScreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt(PPResolution, resolutionDropdown.value);

        OnSettingsChanged?.Invoke();
    }

    public void LoadFromPrefs()
    {
        if (PlayerPrefs.HasKey(PPCamSpeedX))
            CameraSpeedXSlider.value = PlayerPrefs.GetFloat(PPCamSpeedX);

        if (PlayerPrefs.HasKey(PPCamSpeedY))
            CameraSpeedYSlider.value = PlayerPrefs.GetFloat(PPCamSpeedY);

        if (PlayerPrefs.HasKey(PPVolume))
        {
            VolumeSlider.value = PlayerPrefs.GetFloat(PPVolume);
            SetVolume(VolumeSlider.value);
        }

        if (PlayerPrefs.HasKey(PPResolution))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt(PPResolution);
            SetResolution(resolutionDropdown.value);
        }

        if (PlayerPrefs.HasKey(PPFullScreen))
        {
            FullScreenToggle.isOn = PlayerPrefs.GetInt(PPFullScreen) == 1 ? true : false;
            SetFullScreen(FullScreenToggle.isOn);
        }
    }
}
