using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [Header("Player Item")] public GameObject playerListItemViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;
    
    [Header("Lobby")] public ulong currentLobbyID;
    public bool playerItemCreated = false;
    
    private List<PlayerManager> _playerListItems = new List<PlayerManager>();

    #region GetSingleton

    private NetworkManager _manager;

    private NetworkManager Manager
    {
        get
        {
            if (_manager != null) return _manager;
            return _manager = NetworkManager.Instance as NetworkManager;
        }
    }

    #endregion

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        UpdateLobbyName();
        UpdatePlayerList();
        FindLocalPlayer();
    }

    public void UpdateLobbyName()
    {
        currentLobbyID = Manager.currentLobbyID;
    }
    
    public void UpdatePlayerList()
    {
        if (!playerItemCreated) CreateHostPlayerItem();
        if (_playerListItems.Count < Manager.GamePlayer.Count) CreateClientPlayerItem();
        if (_playerListItems.Count > Manager.GamePlayer.Count) RemovePlayerItem();
        if (_playerListItems.Count == Manager.GamePlayer.Count) UpdatePlayerItem();
    }
    
    public void CreateHostPlayerItem()
    {
        foreach (Player player in Manager.GamePlayer)
        {
            GameObject newPlayerItem = Instantiate(playerListItemPrefab, playerListItemViewContent.transform);
            PlayerManager newPlayerItemComponent = newPlayerItem.GetComponent<PlayerManager>();
                
            newPlayerItemComponent.playerNameText.text = player.playerName;
            newPlayerItemComponent.PlayerSteamID = player.playerSteamId;
            newPlayerItemComponent.SetPlayerValues();

            _playerListItems.Add(newPlayerItemComponent);
        }

        playerItemCreated = true;
    }
    
    public void CreateClientPlayerItem()
    {
        foreach (Player player in _manager.GamePlayer)
        {
            if (!_playerListItems.Any(item => item.PlayerSteamID == player.playerSteamId))
            {
                GameObject newPlayerItem = Instantiate(playerListItemPrefab, playerListItemViewContent.transform);
                PlayerManager newPlayerItemComponent = newPlayerItem.GetComponent<PlayerManager>();
                newPlayerItemComponent.playerNameText.text = player.playerName;
                newPlayerItemComponent.PlayerSteamID = player.playerSteamId;
                newPlayerItemComponent.SetPlayerValues();

                _playerListItems.Add(newPlayerItemComponent);
            }
        }
    }
    
    public void RemovePlayerItem()
    {
        _playerListItems.RemoveAll(item =>
            !_manager.GamePlayer.Any(player => player.playerSteamId == item.PlayerSteamID));
    }
    
    public void UpdatePlayerItem()
    {
        foreach (Player player in Manager.GamePlayer)
        {
            foreach (PlayerManager playerList in _playerListItems)
            {
                if (playerList.PlayerSteamID == player.playerSteamId)
                {
                    playerList.playerNameText.text = player.playerName;
                    playerList.SetPlayerValues();
                }
            }
        }
    }
    
    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
    }
}
