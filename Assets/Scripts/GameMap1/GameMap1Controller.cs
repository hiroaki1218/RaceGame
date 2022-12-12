using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameMap1Controller : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI StartTimerText;
    [SerializeField] TextMeshProUGUI[] Text;
    [SerializeField] TextMeshProUGUI MinText;
    [SerializeField] TextMeshProUGUI SecText;
    [SerializeField] TextMeshProUGUI DotText;
    public static GameMap1Controller instance;
    public Transform[] spawnPoints;
    [SerializeField] LoadingScene loadingscene;
    public bool Waiting;
    int allReadyPlayers;
    int allGoalCount;
    bool isMyGoal;
    int myGoalmin;
    int myGoalsec;
    public float min;
    public float sec;
    public int timermin;
    public int timersec;
    int scoremin;
    int scoresec;
    bool sceneChanged;
    bool leavedRoom;

    public static bool startboosttime;
    public static bool isStartedMatch;

    private void Start()
    {
        Waiting = true;
        photonView.RPC(nameof(ReadyCount), RpcTarget.AllViaServer);
        instance = this;
        //Player Spawn
        StartCoroutine(SpawnDelay());
        isMyGoal = false;
        sceneChanged = false;
        leavedRoom = false;
        startboosttime = false;
        isStartedMatch = false;
    }

    //スポーンまで少し時間かける
    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnSystem.instance.SpawnPlayerInGame();
    }

    private void FixedUpdate()
    {
        if (!Waiting)
        {
            if (!leavedRoom)
            {
                if (allGoalCount != PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    //全員準備完了し、まだ全員ゴールしてなかったら、カウント
                    Timer.TimerCountUp();
                }
                else
                {
                    //全員ゴールしてたらシーン移動+チームから抜ける
                    StartCoroutine(SceneChangeWait());
                }

                if (!isMyGoal)
                {
                    //自分がゴールしてなかったら、カウントをテキストに
                    Text[0].text = ":";
                    Text[1].text = ":";
                    MinText.text = GetPointDigit(timermin, 2).ToString() + GetPointDigit(timermin, 1).ToString();   
                    SecText.text = GetPointDigit(timersec, 4).ToString() + GetPointDigit(timersec, 3).ToString();
                    DotText.text = GetPointDigit(timersec, 2).ToString() + GetPointDigit(timersec, 1).ToString();
                }
            }
        }
    }
    private void Update()
    {
        //ゴールした時
        if (Input.GetKeyDown(KeyCode.G))
        {
            isMyGoal = true;
            myGoalmin = timermin;
            myGoalsec = timersec;

            //スコア計算
            scoremin = myGoalmin * 6000;
            scoresec = myGoalsec;
            //PlayFabにスコア送信
            MyPlayerController.instance.SubmitScore(-1 * (scoremin + scoresec));
            photonView.RPC(nameof(GoalCount), RpcTarget.AllViaServer);

            //ゴールした時テキストにタイム固定表示
            Text[0].text = ":";
            Text[1].text = ":";
            MinText.text = GetPointDigit(myGoalmin, 2).ToString() + GetPointDigit(myGoalmin, 1).ToString();
            SecText.text = GetPointDigit(myGoalsec, 4).ToString() + GetPointDigit(myGoalsec, 3).ToString();
            DotText.text = GetPointDigit(myGoalsec, 2).ToString() + GetPointDigit(myGoalsec, 1).ToString();

        }
    }

    IEnumerator SceneChangeWait()
    {
        yield return new WaitForSeconds(5);
        if (!sceneChanged)
        {
            leavedRoom = true;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            loadingscene.LoadNextScene("Ranking");
            sceneChanged = true;
        }
    } 

    [PunRPC]
    void ReadyCount()
    {
        allReadyPlayers++;
        if (allReadyPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            photonView.RPC(nameof(StartCountDown), RpcTarget.AllViaServer);
            //StartCoroutine(StartCountDown());
        }
    }

    [PunRPC]
    IEnumerator StartCountDown()
    {
        //ロード長引いた時間
        yield return new WaitForSeconds(5);
        if (Waiting)
        {
            for (int i = 3; i >= 0; i--)
            {
                if(i != 0)
                {
                    StartTimerText.text = i.ToString();

                    if(i == 2)
                    {
                        BoostController.instance.canjustStartDash = true;
                    }
                    else
                    {
                        BoostController.instance.canjustStartDash = false;
                    }
                }
                else
                {
                    StartTimerText.text = "GO!";
                    startboosttime = true;
                    isStartedMatch = true;
                }
                yield return new WaitForSeconds(1);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
            }

            StartTimerText.text = null;
            Waiting = false;  
        }
        
        if(startboosttime)
        {
            yield return new WaitForSeconds(1);
            startboosttime = false;
        }
    }

    [PunRPC]
    void GoalCount()
    {
        allGoalCount++;
    }

    //指定した桁の値を返す
    public int GetPointDigit(int num, int digit)
    {
        return (int)(num / Mathf.Pow(10, digit - 1)) % 10;
    }
}
