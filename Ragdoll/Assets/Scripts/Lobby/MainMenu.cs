using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SteamLobby steamLobby;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel;
    [SerializeField] private GameObject steamErrorPanel;

    public void HostLobby()
    {
        steamLobby.HostLobby();
        HideLandingPage();
    }

    public void Start()
    {
        if (!SteamManager.Initialized)
        {
            steamErrorPanel.SetActive(true);
            return;
        }
        landingPagePanel.SetActive(true);
        NetworkManagerNinjaRagdoll.OnClientDisconnected += ReloadScene;
        NetworkManagerNinjaRagdoll.OnServerStopped += ReloadScene;
        RoomPlayer.OnLocalPlayerStarted += HideLandingPage;
    }

    private void HideLandingPage()
    {
        landingPagePanel.SetActive(false);
    }


    private void ReloadScene()
    {
        Debug.Log("ReloadScene");
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void OnDestroy()
    {
        NetworkManagerNinjaRagdoll.OnClientDisconnected -= ReloadScene;
        NetworkManagerNinjaRagdoll.OnServerStopped -= ReloadScene;
        RoomPlayer.OnLocalPlayerStarted -= HideLandingPage;
    }
}
