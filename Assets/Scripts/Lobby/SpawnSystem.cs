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

        //�����̒��ł̎����̃i���o�[�J�E���g
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
        Debug.Log($"My Number is { myNumberInRoom }");
        //Player�I�u�W�F�N�g�̃X�|�[��
        myPlayerAvatar = PhotonNetwork.Instantiate("Player", LobbyGameController.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        SetPlayerColor(ColorChange.instance.PickedColorNum);
    }

    public void SpawnPlayerInGame()
    {
        Debug.Log($"My Number is { myNumberInRoom }");
        //Player�I�u�W�F�N�g�̃X�|�[��
        myPlayerAvatar = PhotonNetwork.Instantiate("Player", GameMap1Controller.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        SetPlayerColor(ColorChange.instance.PickedColorNum);
    }

    //�F���J�X�^���v���p�e�B�ɐݒ�
    public void SetPlayerColor(int ColorNum)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties["Color"] = ColorNum;

        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    //�J�X�^���v���p�e�B����F���擾���A�S���ɔ��f
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedColor)
    {
        var properties = changedColor;

        object colorValue = null;
        if (properties.TryGetValue("Color", out colorValue))
        {
            int colorIndex = (int)colorValue;

            // �Q�[�����Player�p�̃I�u�W�F�N�g�̒�����PhotonView��ID���ύX����Player�Ɠ����I�u�W�F�N�g�̐F��ύX����B
            var playerObjects = GameObject.FindGameObjectsWithTag("Player");
            var playerObject = playerObjects.FirstOrDefault(obj => obj.GetComponent<PhotonView>().Owner == targetPlayer);
            playerObject.transform.GetChild(3).gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];
            playerObject.transform.GetChild(6).gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];
            return;
        }
    }
}
