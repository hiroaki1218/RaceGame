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

    [Header("CarController")]
    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    float horizontalInput;
    float verticalInput;
    float currentsteerAngle;
    float currentbreakForce;
    bool isBreaking;

    [SerializeField] float motorForce;
    [SerializeField] float breakForce;
    [SerializeField] float maxSteerAngle;

    [SerializeField] WheelCollider FrontR;
    [SerializeField] WheelCollider FrontL;
    [SerializeField] WheelCollider BackR;
    [SerializeField] WheelCollider BackL;

    bool IsGetComponents;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
    }

    //carの動き
    private void Update()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        FrontL.motorTorque = verticalInput * motorForce;
        FrontR.motorTorque = verticalInput * motorForce;

        currentbreakForce = isBreaking ? breakForce : 0;
        if (isBreaking)
        {
            ApplyBreaking();
        }
    }

    //Break
    private void ApplyBreaking()
    {
        FrontR.brakeTorque = currentbreakForce;
        FrontL.brakeTorque = currentbreakForce;
        BackR.brakeTorque = currentbreakForce;
        BackL.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentsteerAngle = maxSteerAngle * horizontalInput;
        FrontL.steerAngle = currentsteerAngle;
        FrontR.steerAngle = currentsteerAngle;
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
