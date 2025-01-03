using System.Collections.Generic;
using Photon.Pun;
using Steamworks;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;
    
    private void Awake() => Instance = this;
    
    [Header("Lobby")]
    public ulong CurrentLobbyID;
    
    public List<CSteamID> lobbyID = new List<CSteamID>();
    
    public string lobbyName;
    
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequest;
    protected Callback<LobbyEnter_t> lobbyEntered;
    
    protected Callback<LobbyMatchList_t> lobbyList;
    protected Callback<LobbyDataUpdate_t> lobbyData;
    
    private void Start()
    {
        if (!SteamManager.Initialized) return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        lobbyList = Callback<LobbyMatchList_t>.Create(MatchLobby);
        lobbyData = Callback<LobbyDataUpdate_t>.Create(GetLobbyData);
    }
    
    public void FindLobbies()
    {
        if (lobbyID.Count > 0) lobbyID.Clear();

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
        SteamMatchmaking.RequestLobbyList();
    }
    
    void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        Debug.Log("Lobby created");

        CSteamID ulSteamID = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(ulSteamID, "LobbyId",
            SteamUser.GetSteamID().ToString());

        SteamMatchmaking.SetLobbyData(ulSteamID, "name",
            lobbyName + " Lobby");
    }
    
    void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Requested");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    
    void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        if (PhotonNetwork.IsConnectedAndReady == false) return;

        CSteamID ulSteamID = new CSteamID(callback.m_ulSteamIDLobby);
    }
    
    void MatchLobby(LobbyMatchList_t callback)
    {
        for (int i = 0; i < callback.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyID.Add(lobbyId);
            SteamMatchmaking.RequestLobbyData(lobbyId);
        }
    }
    
    void GetLobbyData(LobbyDataUpdate_t callback)
    {
        StartSceneUIManager.Instance.DisplayLobby(lobbyID, callback);
    }
}
