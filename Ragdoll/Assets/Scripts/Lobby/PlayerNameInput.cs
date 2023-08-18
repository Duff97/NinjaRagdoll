using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button continueButton;
    [SerializeField] private int maxNameLenght;

    public static string DisplayName { 
        get 
        {
            return PlayerPrefs.HasKey(PlayerPrefsNameKey) ? 
                PlayerPrefs.GetString(PlayerPrefsNameKey) : 
                "";
        } 
    }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        if (DisplayName == "") { return; }

        string defaultName = DisplayName;

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !(string.IsNullOrEmpty(name) || name.Length > maxNameLenght);
    }

    public void SavePlayerName()
    {
        PlayerPrefs.SetString(PlayerPrefsNameKey, nameInputField.text);
    }
}
