using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Unity.Collections;
using Photon.Pun;

public class Login : MonoBehaviour
{
    [SerializeField] public GameObject FirstLoginCanvas;
    [SerializeField] InputField inputPassword;
    [SerializeField] InputField inputName;
    [SerializeField] Text passnumChr;
    [SerializeField] Text namenumChr;
    [SerializeField] Text ErrorOrSuccessText;

    public static string MyNickName;
    private void Start()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }
    //�o�^
    public void RegisterButton()
    {
        //UserName
        if (inputName.text.Length <= 2)
        {
            namenumChr.color = new Color(1.0f, 0f, 0f, 1.0f);
            Debug.Log("Name�̕�����������������܂���B");
            StartCoroutine(ColorBack());
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Password = "123456",
            Username = inputName.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered And Logged In!");
        ErrorOrSuccessText.text = "�o�^�����O�C�������I";
        NameEnter();
        StartCoroutine(LoginCanvasSetActiveFalse());
        Debug.Log("MyNickName is" + MyNickName);
    }

    //Login
    public void LoginButton()
    {
        //UserName
        if (inputName.text.Length <= 2)
        {
            namenumChr.color = new Color(1.0f, 0f, 0f, 1.0f);
            Debug.Log("Name�̕�����������������܂���B");
            StartCoroutine(ColorBack());
            return;
        }
        //�����Ă邩�ǂ���
        var request = new LoginWithPlayFabRequest
        {
            Password = "123456",
            Username = inputName.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnError);
    } 
        
    //�F���߂�܂Ő��b�܂�
    IEnumerator ColorBack()
    {
        yield return new WaitForSeconds(1);
        passnumChr.color = new Color(1,1,1);
        namenumChr.color = new Color(1, 1, 1);
    }
    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged In!");
        ErrorOrSuccessText.text = "���O�C�������I";
        MyNickName = result.InfoResultPayload.PlayerProfile.DisplayName;
        Debug.Log("MyNickName is" + MyNickName);
        StartCoroutine(LoginCanvasSetActiveFalse());
    }
    //���O�C����ʏ����܂Ő��b�܂�
    IEnumerator LoginCanvasSetActiveFalse()
    {
        yield return new WaitForSeconds(2);
        FirstLoginCanvas.SetActive(false);
    }

    //���͖��ɊĎ�
    public void LineCount()
    {
        //���s�h�~
        if (inputName.text.IndexOf("\n") == -1)
        {
            return;
        }
        inputName.text = inputName.text.Trim();
    }

    //�����{�^��
    public void NameEnter()
    {
        //���O�̓o�^
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = inputName.text
        }, result =>
        {
            MyNickName = result.DisplayName;
            Debug.Log("MyNickName is�F" + result.DisplayName);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            ErrorOrSuccessText.text = "���̃��[�U�[�l�[���͊��Ɏg���Ă��܂��B";
        }
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            ErrorOrSuccessText.text = "���[�U�[�l�[�����Ԉ���Ă��܂��B";
        }
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            ErrorOrSuccessText.text = "���[�U�[�l�[�����Ԉ���Ă��܂��B";
        }
    }
}
