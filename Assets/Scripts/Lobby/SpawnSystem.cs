using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnSystem : MonoBehaviourPunCallbacks
{
    public Renderer CarRenderer;
    PhotonView myPV;
    GameObject myPlayerAvatar;

    int myNumberInRoom;
    public static SpawnSystem instance;

    void Start()
    {
        instance = this;
        myPV = GetComponent<PhotonView>();

        //部屋の中での自分のナンバーカウント
        foreach (Player p in MenuGameManager.allPlayers)
        {
            Debug.Log(p);
            if (p != PhotonNetwork.LocalPlayer)
            {
                myNumberInRoom++;
            }
        }
        DontDestroyOnLoad(this);
    }

    public void SpawnPlayerInLobby()
    {
        if (myPV.IsMine)
        {
            Debug.Log($"My Number is { myNumberInRoom }");
            //Playerオブジェクトのスポーン
            myPlayerAvatar = PhotonNetwork.Instantiate("Player", LobbyGameController.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
            ColorSet();
        }         
    }

    public void SpawnPlayerInGame()
    {
        if (myPV.IsMine)
        {
            Debug.Log($"My Number is { myNumberInRoom }");
            //Playerオブジェクトのスポーン
            myPlayerAvatar = PhotonNetwork.Instantiate("Player", GameMap1Controller.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        }
    }

    [PunRPC]
    void ColorSet()
    {
        if (myPV.IsMine)
        {
            CarRenderer = GameObject.Find("Player(Clone)").GetComponent<Renderer>();
            CarRenderer.material.color = Color.blue;
        }
    }
}
