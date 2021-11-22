using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    Vector3 Vec;
    //public string userId;
    public int viewId;
    public string mood;

    //public List<GameObject> avatars = new List<GameObject>();

    GameRoomStatus gameRoomStatus;

    void Awake()
    {
        gameRoomStatus = FindObjectOfType<GameRoomStatus>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            //userId = PhotonNetwork.LocalPlayer.UserId;
            viewId = photonView.ViewID;
            mood = (string)PhotonNetwork.LocalPlayer.CustomProperties["mood"];

            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["viewId"] = photonView.ViewID;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            photonView.RPC("AddAvatar", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //GetComponent<PhotonView>().RPC("RemovePlayer", RpcTarget.All, viewId);
    }

    void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            Vec = transform.localPosition;
            Vec.x += Input.GetAxis("Horizontal") * Time.deltaTime * 10;
            Vec.z += Input.GetAxis("Vertical") * Time.deltaTime * 10;
            transform.localPosition = Vec;
        }
    }

    [PunRPC]
    public void AddAvatar(Player player)
    {
        CreatePlayerAvatar(player.CustomProperties);
        // Recreate all missing avatars
        foreach(Player _player in PhotonNetwork.PlayerList)
        {
            if ((int)_player.CustomProperties["viewId"] != viewId)
            {
                CreatePlayerAvatar(_player.CustomProperties);
            }
            //Debug.Log(_player.CustomProperties["viewId"] + " - " + _player.CustomProperties["mood"] + " - " + _player.UserId);
        }
    }

    public void CreatePlayerAvatar(Hashtable hash)
    {
        foreach (GameObject ava in gameRoomStatus.avatars)
        {
            // Instantiate the right avatar and set its parent a gameobject of a player who owns it
            if (ava.gameObject.name == (string)hash["mood"])
            {
                foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    // Create new avatars
                    if (player.GetComponent<PhotonView>().ViewID == (int)hash["viewId"])
                    {
                        // Map mood and id to the playercontroller for debugging
                        player.GetComponent<PlayerController>().mood = (string)hash["mood"];
                        player.GetComponent<PlayerController>().viewId = (int)hash["viewId"];
                        // Remove previous avatars if any exist
                        for (int i = 0; i < player.transform.Find("Face").childCount; i++)
                        {
                            Destroy(player.transform.Find("Face").GetChild(i).gameObject);
                        }
                        GameObject avatar = Instantiate(ava, player.transform.Find("Face").position, player.transform.Find("Face").rotation);
                        avatar.transform.SetParent(player.transform.Find("Face"));
                    }
                }
            }
        }
    }
}
