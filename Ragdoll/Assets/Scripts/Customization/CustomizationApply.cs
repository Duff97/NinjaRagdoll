using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationApply : MonoBehaviour
{
    [SerializeField] private GameObject[] hats;
    [HideInInspector] public int hatIndex;

    [Header("References")]
    [SerializeField] private GameObject headObj;

    [Header("Parameters")]
    [SerializeField] private bool loadOnStart;

    private const string PlayerPrefHatString = "CustomHat";

    // Start is called before the first frame update
    void Start()
    {
        if (loadOnStart)
            LoadFromPrefs();
    }

    public void RemoveCustomization()
    {
        foreach (Transform child in headObj.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ApplyCustomization()
    {
        RemoveCustomization();
        if (hatIndex != -1)
        {
            GameObject hat = Instantiate(hats[hatIndex]);
            hat.transform.SetParent(headObj.transform, false);
            hat.SetActive(true);
        }
    }

    public void SaveToPrefs()
    {
        PlayerPrefs.SetInt(PlayerPrefHatString, hatIndex);
    }

    public void LoadFromPrefs()
    {
        if (PlayerPrefs.HasKey(PlayerPrefHatString))
            hatIndex = PlayerPrefs.GetInt(PlayerPrefHatString);
        else
            hatIndex = -1;

        ApplyCustomization();
    }
}
