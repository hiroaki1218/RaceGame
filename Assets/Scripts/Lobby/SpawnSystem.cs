using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class SpawnSystem : MonoBehaviourPunCallbacks
{
    PhotonView myPV;
    GameObject myPlayerAvatar;

    public int myNumberInRoom;
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
        //名前をカスタムプロパティに設定
        SetPlayerName(Login.MyNickName);
        Debug.Log($"My Number is { myNumberInRoom }");
        //Playerオブジェクトのスポーン
        myPlayerAvatar = PhotonNetwork.Instantiate("Player", LobbyGameController.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        //色をカスタムプロパティに設定
        SetPlayerColor(ColorChange.instance.PickedColorNum);
    }

    public void SpawnPlayerInGame()
    {
        //名前をカスタムプロパティに設定
        SetPlayerName(Login.MyNickName);
        Debug.Log($"My Number is { myNumberInRoom }");
        //Playerオブジェクトのスポーン
        myPlayerAvatar = PhotonNetwork.Instantiate("Player", GameMap1Controller.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        SetPlayerColor(ColorChange.instance.PickedColorNum);
    }

    //名前をカスタムプロパティに設定
    public void SetPlayerName(string PlayerName)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties["N"] = PlayerName;

        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    //色をカスタムプロパティに設定
    public void SetPlayerColor(int ColorNum)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties["C"] = ColorNum;

        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    //カスタムプロパティから色を取得し、全員に反映
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedColor)
    {
        var properties = changedColor;

        object colorValue = null;
        if (properties.TryGetValue("C", out colorValue))
        {
            int colorIndex = (int)colorValue;

            // ゲーム上のPlayer用のオブジェクトの中からPhotonViewのIDが変更したPlayerと同じオブジェクトの色を変更する。
            var playerObjects = GameObject.FindGameObjectsWithTag("Player");
            var playerObject = playerObjects.FirstOrDefault(obj => obj.GetComponent<PhotonView>().Owner == targetPlayer);

            //カラー
            playerObject.transform.GetChild(3).gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];
            playerObject.transform.GetChild(6).gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];

            //Name
            string PlayerName = GetPlayerName(targetPlayer);
            GameObject parent = playerObject.transform.GetChild(9).gameObject;
            GameObject child = parent.transform.GetChild(0).gameObject;
            TextMeshProUGUI playerNameText = child.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            playerNameText.text = PlayerName;
            return;
        }
    }

    //カスタムプロパティから名前を返す
    public string GetPlayerName(Player player)
    {
        return (player.CustomProperties["N"] is string name) ? name : string.Empty;
    }
}
