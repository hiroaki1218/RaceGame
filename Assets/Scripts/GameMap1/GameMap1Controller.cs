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
    bool Waiting;
    int allReadyPlayers;
    int allGoalCount;
    bool isMyGoal;
    int myGoalmin;
    int myGoalsec;
    float min;
    float sec;
    int timermin;
    int timersec;
    int scoremin;
    int scoresec;
    bool sceneChanged;
    bool leavedRoom;

    private void Start()
    {
        Waiting = true;
        photonView.RPC(nameof(ReadyCount), RpcTarget.AllViaServer);
        instance = this;
        SpawnSystem.instance.SpawnPlayerInGame();
        isMyGoal = false;
        sceneChanged = false;
        leavedRoom = false;
    }

    private void FixedUpdate()
    {
        if (!Waiting)
        {
            if (!leavedRoom)
            {
                if (allGoalCount != PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    //�S�������������A�܂��S���S�[�����ĂȂ�������A�J�E���g
                    photonView.RPC(nameof(TimerCountUp), RpcTarget.AllViaServer);
                }
                else
                {
                    //�S���S�[�����Ă���V�[���ړ�+�`�[�����甲����
                    StartCoroutine(SceneChangeWait());
                }

                if (!isMyGoal)
                {
                    //�������S�[�����ĂȂ�������A�J�E���g���e�L�X�g��
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
        //�S�[��������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMyGoal = true;
            myGoalmin = timermin;
            myGoalsec = timersec;

            //�X�R�A�v�Z
            scoremin = myGoalmin * 6000;
            scoresec = myGoalsec;
            //PlayFab�ɃX�R�A���M
            MyPlayerController.instance.SubmitScore(-1 * (scoremin + scoresec));
            photonView.RPC(nameof(GoalCount), RpcTarget.AllViaServer);

            //�S�[���������e�L�X�g�Ƀ^�C���Œ�\��
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
            StartCoroutine(StartCountDown());
        }
    }
    IEnumerator StartCountDown()
    {
        if (Waiting)
        {
            for (int i = 3; i >= 0; i--)
            {
                if(i != 0)
                {
                    StartTimerText.text = i.ToString();
                }
                else
                {
                    StartTimerText.text = "GO!";
                }
                yield return new WaitForSeconds(1);
            }
            Waiting = false;
            StartTimerText.text = null;
        }
    }
    [PunRPC]
    void TimerCountUp()
    {
        //�ŏ��̃J�E���g�_�E�����I�������J�n 
        sec += Time.deltaTime;
        timersec = (int)(sec * 100);
        if (sec >= 60)
        {
            timermin++;
            sec = 0;
        }
    }
    [PunRPC]
    void GoalCount()
    {
        allGoalCount++;
    }

    //�w�肵�����̒l��Ԃ�
    public int GetPointDigit(int num, int digit)
    {
        return (int)(num / Mathf.Pow(10, digit - 1)) % 10;
    }
}
