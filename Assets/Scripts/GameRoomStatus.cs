using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameRoomStatus : MonoBehaviourPunCallbacks
{
    PlayerStatus playerStatus;
    public List<GameObject> avatars = new List<GameObject>();

    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        playerStatus.LoadPlayer();
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect to Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        // Share your avatar data among all players so they can instantiate you accordingly
        Hashtable hash = new Hashtable();
        hash["mood"] = playerStatus.face;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        base.OnConnectedToMaster();

        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("PlayerPrefab", transform.position, transform.rotation);
        base.OnJoinedRoom();
    }
}
