using System;
using Photon.Pun;
using Steamworks;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    public string playerName;
    public ulong playerSteamId;

    private void Start()
    {
        OnAwakeRPC();

        if (photonView.IsMine)
        {
            PhotonNetwork.NickName = SteamFriends.GetPersonaName();
            DontDestroyOnLoad(this);
        }
    }

    [PunRPC]
    void OnAwakeRPC()
    {
        Debug.Log("Hello");
    }
}
