using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton

    private static NetworkManager _instance;

    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("NetworkManager instance is null! Make sure it exists in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion
    
    public List<Player> GamePlayer = new List<Player>();

    public GameObject playerPrefab;
    
    public ulong currentLobbyID;
    
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");

        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        GameObject playerObj = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
        playerObj.name = "LocalGamePlayer";
        Player player = playerObj.GetComponent<Player>();
        player.playerSteamId = SteamUser.GetSteamID().m_SteamID;
        player.playerName = SteamFriends.GetPersonaName();
        GamePlayer.Add(player);
    }
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);
        
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);
    }
}
