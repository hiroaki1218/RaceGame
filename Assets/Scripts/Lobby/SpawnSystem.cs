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
    MyHP _myHP;

    public int myNumberInRoom;
    public static SpawnSystem instance;

    bool GetComponentsPlayer;

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
        //Player�I�u�W�F�N�g�̃X�|�[��
        myPlayerAvatar = PhotonNetwork.Instantiate("Player", LobbyGameController.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        //�܂�HP��UI�̃R���|�[�l���g���擾���Ă��Ȃ�
        GetComponentsPlayer = false;
        //���O���J�X�^���v���p�e�B�ɐݒ�
        SetPlayerName(Login.MyNickName);
        Debug.Log($"My Number is { myNumberInRoom }");
        //�I�������L�����̎�ނ��J�X�^���v���p�e�B�ɐݒ�
        SetCharacterRole(CharacterRoleChange.instance.state);
        //�F���J�X�^���v���p�e�B�ɐݒ�
        SetPlayerColor(ColorChange.instance.PickedColorNum);
    }

    public void SpawnPlayerInGame()
    {
        //Player�I�u�W�F�N�g�̃X�|�[��
        myPlayerAvatar = PhotonNetwork.Instantiate("Player", GameMap1Controller.instance.spawnPoints[myNumberInRoom].position, Quaternion.identity);
        //�܂�HP��UI�̃R���|�[�l���g���擾���Ă��Ȃ�
        GetComponentsPlayer = false;
        //���O���J�X�^���v���p�e�B�ɐݒ�
        SetPlayerName(Login.MyNickName);
        Debug.Log($"My Number is { myNumberInRoom }");
        //�I�������L�����̎�ނ��J�X�^���v���p�e�B�ɐݒ�
        SetCharacterRole(CharacterRoleChange.instance.state);
        //�F���J�X�^���v���p�e�B�ɐݒ�
        SetPlayerColor(ColorChange.instance.PickedColorNum);
    }

    //�����A�S���̃X�|�[�����������Ă�����AHP��UI�̃R���|�[�l���g���擾
    //private void FixedUpdate()
    //{
        //if(SceneManager.GetActiveScene().name != "Menu")
        //{
            //if (!GetComponentsPlayer)
            //{
                //if (IsSpawnedPlayer())
                //{
                    //if (myPV.IsMine)
                    //{
                        //Debug.Log("SpawnSystem mine");
                        
                        //GetComponentsPlayer = true;
                    //}
                //}
            //}
        //}
    //}

    //���O���J�X�^���v���p�e�B�ɐݒ�
    public void SetPlayerName(string PlayerName)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties["N"] = PlayerName;

        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    //�F���J�X�^���v���p�e�B�ɐݒ�
    public void SetPlayerColor(int ColorNum)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties["C"] = ColorNum;

        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    //�L�������J�X�^���v���p�e�B�ɐݒ�
    public void SetCharacterRole(int CharacterNum)
    {
        var properties = new ExitGames.Client.Photon.Hashtable();
        properties["R"] = CharacterNum;

        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    //�J�X�^���v���p�e�B����v�f���擾���A�S���ɔ��f
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedColor)
    {
        var properties = changedColor;

        object colorValue = null; 

        if (properties.TryGetValue("C", out colorValue))
        {
            int colorIndex = (int)colorValue;

            // �Q�[�����Player�p�̃I�u�W�F�N�g�̒�����PhotonView��ID���ύX����Player�Ɠ����I�u�W�F�N�g�̐F��ύX����B
            var playerObjects = GameObject.FindGameObjectsWithTag("Player");
            var playerObject = playerObjects.FirstOrDefault(obj => obj.GetComponent<PhotonView>().Owner == targetPlayer);

            //�J���[
            playerObject.transform.GetChild(3).gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];
            playerObject.transform.GetChild(6).gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];

            //�L����
            int CharacterNum = GetPlayerPickedCharacter(targetPlayer);
            GameObject CharacterParent = playerObject.transform.Find("MyCharacter").gameObject;
            CharacterParent.transform.GetChild(CharacterNum).gameObject.SetActive(true);

            //Name
            string PlayerName = GetPlayerName(targetPlayer);
            GameObject parent = playerObject.transform.GetChild(9).gameObject;
            GameObject child = parent.transform.GetChild(0).gameObject;
            TextMeshProUGUI playerNameText = child.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            playerNameText.text = PlayerName;

            //Camera UI
            if(targetPlayer != PhotonNetwork.LocalPlayer)
            {
                GameObject myUI = playerObject.transform.Find("MyUI").gameObject;
                GameObject myCamera = playerObject.transform.Find("MyCamera").gameObject;

                myUI.SetActive(false);
                myCamera.SetActive(false);

                _myHP = playerObject.GetComponent<MyHP>();
                _myHP.GetComponentsHP();
            }
            else
            {
                GameObject toOtherUI = playerObject.transform.Find("toOtherUI").gameObject;
                toOtherUI.SetActive(false);
                _myHP = playerObject.GetComponent<MyHP>();
                _myHP.GetComponentsHP();
            }

            return;
        }
    }

    //�J�X�^���v���p�e�B���疼�O��Ԃ�
    public string GetPlayerName(Player player)
    {
        return (player.CustomProperties["N"] is string name) ? name : string.Empty;
    }

    //�J�X�^���v���p�e�B����L�����̎�ނ�Ԃ�
    public int GetPlayerPickedCharacter(Player player)
    {
        return (player.CustomProperties["R"] is int chr) ? chr : Random.Range(0,5);
    }

    //�X�|�[���������ǂ���
    public bool IsSpawnedPlayer()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
