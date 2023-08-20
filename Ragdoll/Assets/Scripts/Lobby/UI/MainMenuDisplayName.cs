using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuDisplayName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayNameText;

    private void OnGUI()
    {
        displayNameText.text = "Playing as " + PlayerNameInput.DisplayName;
    }
}
