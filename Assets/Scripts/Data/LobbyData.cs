using Photon.Pun;
using Steamworks;
using TMPro;
using UnityEngine;

public class LobbyData : MonoBehaviour
{
    public string lobbyName;

    public TextMeshProUGUI lobbyNameText;

    public CSteamID lobbyId;

    public void JoinLobby()
    {
        StartSceneUIManager.Instance.JoinLobby(lobbyId);
    }

    public void SetLobbyData()
    {
        lobbyNameText.text = lobbyName;
    }
}
