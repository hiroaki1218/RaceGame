using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;
using TMPro;

public class MyPlayerController : MonoBehaviourPunCallbacks
{
    public static MyPlayerController instance;
    GameObject myUI;
    GameObject myCamera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
    }

    //プレイヤースポーン後のUI、カメラの管理
    void FixedUpdate()
    {
        myUI = transform.Find("UI").gameObject;
        myCamera = transform.Find("Camera").gameObject;

        if (photonView.IsMine)
        {
            myUI.SetActive(false);
            myCamera.SetActive(true);
        }
        else
        {
            myUI.SetActive(true);
            myCamera.SetActive(false);
        }
    }

    public void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "レースタイム",
                    Value = playerScore
                }
            }
        }, result =>
        {
            Debug.Log($"スコア {playerScore} 送信完了！");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
