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
    public void DisplaySteamInfo(CSteamID steamID)
    {
        nameText.text = SteamFriends.GetFriendPersonaName(steamID);
        statusText.text = "Online";

        FriendGameInfo_t friendGameInfo;
        
        if (SteamFriends.GetFriendGamePlayed(steamID, out friendGameInfo))
        {
            if (friendGameInfo.m_gameID.ToString().Equals(steamAppId))
            {
                transform.SetSiblingIndex(0);
                statusText.text = "In game";
                ToggleJoinButton(steamID);
            }
        }

        int imageId = SteamFriends.GetLargeFriendAvatar(steamID);

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

    private void ToggleJoinButton(CSteamID steamId)
    {
        if (SteamFriends.GetFriendGamePlayed(steamId, out FriendGameInfo_t friendGameInfo) && friendGameInfo.m_steamIDLobby.IsValid())
        {
            Debug.Log("Friend is in a game");
        }
    }
}
