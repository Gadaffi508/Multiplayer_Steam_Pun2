using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUIManager : MonoBehaviour
{
    private RoomOptions _options;
    
    public void CreateLobby()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(SteamFriends.GetPersonaName());
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("Photon client is not ready to create a room.");
        }
    }

    public void JoinLobby(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRandomLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void SetLobbyOptions(RoomOptions roomOptions)
    {
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 4;
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
