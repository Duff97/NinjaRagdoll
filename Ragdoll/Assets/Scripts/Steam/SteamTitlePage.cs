using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamTitlePage : MonoBehaviour
{
    public GameObject errorPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized)
            errorPanel.SetActive(true);
        else
            SceneManager.LoadScene("MainMenu");
    }
}
