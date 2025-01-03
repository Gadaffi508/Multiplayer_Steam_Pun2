using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUIManager : MonoBehaviour
{
    public static StartSceneUIManager Instance;
    
    private void Awake() => Instance = this;
    
    public GameObject lobbyDataPrefab;
    public Transform lobbiesMenuContent;

    public List<GameObject> lobbyList = new List<GameObject>();
    
    private RoomOptions _options;
    
    public void CreateLobby()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(SteamFriends.GetPersonaName());
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("Photon client is not ready to create a room.");
        }
    }

    public void JoinLobby(CSteamID _lobbyID)
    {
        PhotonNetwork.JoinRoom(_lobbyID.ToString());
        SteamMatchmaking.JoinLobby(_lobbyID);
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
    
    public void FindLobby()
    {
        SteamLobby.Instance.FindLobbies();
    }
    
    public void DisplayLobby(List<CSteamID> lobbyID, LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyID.Count; i++)
        {
            if (lobbyID[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                GameObject lobby = Instantiate(lobbyDataPrefab, lobbiesMenuContent);
                LobbyData data = lobby.GetComponent<LobbyData>();
                
                data.lobbyId = (CSteamID)lobbyID[i].m_SteamID;
                data.lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyID[i].m_SteamID, "name");
                data.SetLobbyData();
                
                lobbyList.Add(lobby);
            }
        }
    }

    public void ClearLobby()
    {
        foreach (GameObject lobby in lobbyList)
        {
            Destroy(lobby);
        }
        lobbyList.Clear();
    }
    
    public void LeaveRoom(CSteamID lobbyID)
    {
        PhotonNetwork.LeaveRoom();
        SteamMatchmaking.LeaveLobby(lobbyID);
    }
}
