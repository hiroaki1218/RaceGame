using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] LoadingScene loadingScene;
    [SerializeField] GameObject MatchStartButton;
    [SerializeField] GameObject MatchCancelButton;
    [SerializeField] TextMeshProUGUI stateText;
    public static Player[] allPlayers;
    bool inRoom;
    bool isMatched;

    private void Start()
    {
        MatchStartButton.SetActive(true);
        MatchCancelButton.SetActive(false);
        inRoom = false;
        isMatched = false;
        stateText.text = null;
    }

    //�}�b�`�J�n�{�^���������Ƃ�
    public void OnClickMatchButton()
    {
        stateText.text = "�}�b�`��������...";
        MatchStartButton.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    //�}�b�`�L�����Z���{�^���������Ƃ�
    public void OnClickMatchCancelButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    //Room���甲�����Ƃ�
    public override void OnLeftRoom()
    {
        inRoom = false;
        stateText.text = null;
        PhotonNetwork.Disconnect();
        MatchStartButton.SetActive(true);
        MatchCancelButton.SetActive(false);
        Debug.Log("�L�����Z�����܂���");
    }

    //�}�X�^�[�T�[�o�[�ւ̐ڑ�������
    public override void OnConnectedToMaster()
    {
        //Random�ȕ����ɓ���
        PhotonNetwork.JoinRandomRoom();
    }

    //Room�ɓ������Ƃ�
    public override void OnJoinedRoom()
    {
        MatchCancelButton.SetActive(true);
        allPlayers = PhotonNetwork.PlayerList;
        Debug.Log("Joined Lobby!");
        Debug.Log(allPlayers.Length);
        inRoom = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //�������Ȃ������玩���ō��
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2}, TypedLobby.Default);
    }

    //������l��������V�[���ړ�
    private void Update()
    {
        if (!isMatched)
        {
            if (inRoom)
            {
                if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    //�r���Q���h�~
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    //�}�b�`�L�����Z���{�^������
                    MatchCancelButton.SetActive(false);
                    //�X�|�[���V�X�e���I�u�W�F�N�g�̃X�|�[��
                    PhotonNetwork.Instantiate("SpawnSystem", new Vector3(0, 0, 0), Quaternion.identity);
                    isMatched = true;
                    //���r�[�V�[����
                    loadingScene.LoadNextScene("Lobby");
                    Debug.Log("SceneChanged");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (inRoom)
        {
            stateText.text = $"�ҋ@��:{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
    }
}
