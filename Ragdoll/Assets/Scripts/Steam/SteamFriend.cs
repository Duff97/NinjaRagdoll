using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SteamFriend : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private string steamAppId;
    [SerializeField] private RawImage profileImage;
    [SerializeField] private Button joinButton;

    private CSteamID steamId;
    public void DisplaySteamInfo(CSteamID steamId)
    {
        this.steamId = steamId;
        nameText.text = SteamFriends.GetFriendPersonaName(steamId);
        statusText.text = "Online";

        FriendGameInfo_t friendGameInfo;
        
        if (SteamFriends.GetFriendGamePlayed(steamId, out friendGameInfo))
        {
            if (friendGameInfo.m_gameID.ToString().Equals(steamAppId))
            {
                transform.SetSiblingIndex(0);
                statusText.text = "In menu";
                ToggleJoinButton();
            }
        }

        int imageId = SteamFriends.GetLargeFriendAvatar(steamId);

        if (imageId != -1)
        {
            profileImage.texture = GetSteamImageAsTexture(imageId);
        }

    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }

        return texture;
    }

    private void ToggleJoinButton()
    {
        if (SteamFriends.GetFriendGamePlayed(steamId, out FriendGameInfo_t friendGameInfo) && friendGameInfo.m_steamIDLobby.IsValid())
        {
            statusText.text = "In lobby";
            joinButton.gameObject.SetActive(true);
        }
    }

    public void JoinGame()
    {
        Debug.Log("Trying to join game");
        if (SteamFriends.GetFriendGamePlayed(steamId, out FriendGameInfo_t friendGameInfo) && friendGameInfo.m_steamIDLobby.IsValid())
        {
            
            SteamMatchmaking.JoinLobby(friendGameInfo.m_steamIDLobby);
        }
    }
}
