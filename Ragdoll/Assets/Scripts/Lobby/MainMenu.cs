using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerNinjaRagdoll networkManager;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel;
    [SerializeField] private GameObject playerInputPage;

    public void HostLobby()
    {
        networkManager.StartHost();

        landingPagePanel.SetActive(false);
    }

    public void Start()
    {
        landingPagePanel.SetActive(PlayerNameInput.DisplayName != "");
        playerInputPage.SetActive(PlayerNameInput.DisplayName == "");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
