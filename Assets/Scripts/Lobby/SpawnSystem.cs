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

    public bool isAllSpawned;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        isAllSpawned = false;
        myPV = GetComponent<PhotonView>();

        //�����̒��ł̎����̃i���o�[�J�E���g
        foreach (Photon.Realtime.Player p in MenuGameManager.allPlayers)
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
        //���O���J�X�^���v���p�e�B�ɐݒ�
        SetPlayerName(Login.MyNickName);
        Debug.Log($"My Number is { myNumberInRoom }");
        //�I�������L�����̎�ނ��J�X�^���v���p�e�B�ɐݒ�
        SetCharacterRole(CharacterRoleChange.instance.state);
        //�F���J�X�^���v���p�e�B�ɐݒ�
        SetPlayerColor(ColorChange.instance.PickedColorNum);
    }

    //�����A�S���̃X�|�[�����������Ă�����
    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name != "Menu")
        { 
            if (IsSpawnedPlayer())
            {
                isAllSpawned = true;
            }
            else
            {
                isAllSpawned = false;
            }
        }
    }

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
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedColor)
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
            GameObject oparent = playerObject.transform.Find("Car").gameObject;
            oparent.transform.Find("Outside0").gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];
            oparent.transform.Find("Outside1").gameObject.GetComponent<Renderer>().material.color = ColorChange.instance.PLAYER_COLOR[colorIndex];

            //�L����
            int CharacterNum = GetPlayerPickedCharacter(targetPlayer);
            GameObject CharacterParent = playerObject.transform.Find("MyCharacter").gameObject;
            CharacterParent.transform.GetChild(CharacterNum).gameObject.SetActive(true);

            //Name
            string PlayerName = GetPlayerName(targetPlayer);
            GameObject nparent = playerObject.transform.GetChild(1).gameObject;
            GameObject child = nparent.transform.GetChild(0).gameObject;
            TextMeshProUGUI playerNameText = child.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            playerNameText.text = PlayerName;

            //Camera UI
            if(targetPlayer != PhotonNetwork.LocalPlayer)
            {
                GameObject myUI = playerObject.transform.Find("MyUI").gameObject;
                //GameObject myCamera = playerObject.transform.Find("MyCamera").gameObject;

                myUI.SetActive(false);
                //myCamera.SetActive(false);

                _myHP = playerObject.GetComponent<MyHP>();
                _myHP.GetComponentsHP();
            }
            else
            {
                //Camera(new)
                MyCameraFollow.target = playerObject.transform;
                MyCameraFollow.instance.inputmanager = playerObject.GetComponent<InputManager>();
                //Boost
                BoostController.instance.inputmanager = playerObject.GetComponent<InputManager>(); 
                GameObject toOtherUI = playerObject.transform.Find("toOtherUI").gameObject;
                toOtherUI.SetActive(false);
                _myHP = playerObject.GetComponent<MyHP>();
                _myHP.GetComponentsHP();
            }

            return;
        }
    }

    //�J�X�^���v���p�e�B���疼�O��Ԃ�
    public string GetPlayerName(Photon.Realtime.Player player)
    {
        return (player.CustomProperties["N"] is string name) ? name : string.Empty;
    }

    //�J�X�^���v���p�e�B����L�����̎�ނ�Ԃ�
    public int GetPlayerPickedCharacter(Photon.Realtime.Player player)
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
