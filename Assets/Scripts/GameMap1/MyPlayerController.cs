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
using UnityEngine.UI;

public class MyPlayerController : MonoBehaviourPunCallbacks
{
    public static MyPlayerController instance;
    GameObject myUI;
    GameObject toOtherUI;
    GameObject myCamera;

    bool IsGetComponents;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
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
                    StatisticName = "���[�X�^�C��",
                    Value = playerScore
                }
            }
        }, result =>
        {
            Debug.Log($"�X�R�A {playerScore} ���M�����I");
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
