using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class JoinLobbyMenu : MonoBehaviour
    {

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private TMP_InputField ipAddressInputField = null;
        [SerializeField] private Button joinButton = null;

        private NetworkManagerNinjaRagdoll Room
        {
            get
            {
                return NetworkManager.singleton as NetworkManagerNinjaRagdoll;
            }
        }

        private void OnEnable()
        {
            NetworkManagerNinjaRagdoll.OnClientConnected += HandleClientConnected;
            NetworkManagerNinjaRagdoll.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerNinjaRagdoll.OnClientConnected -= HandleClientConnected;
            NetworkManagerNinjaRagdoll.OnClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAddress = ipAddressInputField.text;

            Room.networkAddress = ipAddress;
            Room.StartClient();

            joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true;

            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }
    }
}
