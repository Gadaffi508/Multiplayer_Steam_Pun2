using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public ulong PlayerSteamID;
    public TextMeshProUGUI playerNameText;
    public RawImage playerIcon;
    
    private bool AvatarReceived;
    
    protected Callback<AvatarImageLoaded_t> ImageLoaded;
    
    private void Start()
    {
        PlayerSteamID = SteamUser.GetSteamID().m_SteamID;
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
        SetPlayerValues();
    }
    
    public void SetPlayerValues()
    {
        playerNameText.text = SteamFriends.GetPersonaName();
        if (!AvatarReceived) { GetPlayerIcon(); }
    }
    void GetPlayerIcon()
    {
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        if (ImageID == -1) { return; }
        playerIcon.texture = GetSteamImageAsTexture(ImageID);
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

                Color32[] pixels = texture.GetPixels32();
                for (int y = 0; y < height / 2; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int topIndex = y * (int)width + x;
                        int bottomIndex = ((int)height - 1 - y) * (int)width + x;
                        Color32 temp = pixels[topIndex];
                        pixels[topIndex] = pixels[bottomIndex];
                        pixels[bottomIndex] = temp;
                    }
                }
                texture.SetPixels32(pixels);
                texture.Apply();
            }
        }
        AvatarReceived = true;
        return texture;
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID.m_SteamID == PlayerSteamID)
        {
            playerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        else
        {
            return;
        }
    }
}
