using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerNinjaRagdoll networkManager;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel;
    [SerializeField] private GameObject playerInputPage;

    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            return NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    public void HostLobby()
    {
        Room.StartHost();

        landingPagePanel.SetActive(false);
    }

    public void Start()
    {
        landingPagePanel.SetActive(PlayerNameInput.DisplayName != "");
        playerInputPage.SetActive(PlayerNameInput.DisplayName == "");
        NetworkManagerNinjaRagdoll.OnClientDisconnected += ReloadScene;
    }


    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
