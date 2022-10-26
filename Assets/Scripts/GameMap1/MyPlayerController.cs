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

    //�v���C���[�X�|�[�����UI�A�J�����̊Ǘ�
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
