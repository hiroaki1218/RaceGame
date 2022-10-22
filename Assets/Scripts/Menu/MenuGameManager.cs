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

    //マッチ開始ボタン押したとき
    public void OnClickMatchButton()
    {
        stateText.text = "マッチを検索中...";
        MatchStartButton.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    //マッチキャンセルボタン押したとき
    public void OnClickMatchCancelButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    //Roomから抜けたとき
    public override void OnLeftRoom()
    {
        inRoom = false;
        stateText.text = null;
        PhotonNetwork.Disconnect();
        MatchStartButton.SetActive(true);
        MatchCancelButton.SetActive(false);
        Debug.Log("キャンセルしました");
    }

    //マスターサーバーへの接続完了時
    public override void OnConnectedToMaster()
    {
        //Randomな部屋に入る
        PhotonNetwork.JoinRandomRoom();
    }

    //Roomに入ったとき
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
        //部屋がなかったら自分で作る
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2}, TypedLobby.Default);
    }

    //もし二人だったらシーン移動
    private void Update()
    {
        if (!isMatched)
        {
            if (inRoom)
            {
                if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    //途中参加防止
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    //マッチキャンセルボタン消す
                    MatchCancelButton.SetActive(false);
                    //スポーンシステムオブジェクトのスポーン
                    PhotonNetwork.Instantiate("SpawnSystem", new Vector3(0, 0, 0), Quaternion.identity);
                    isMatched = true;
                    //ロビーシーンへ
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
            stateText.text = $"待機中:{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
    }
}
