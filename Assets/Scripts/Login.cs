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
using TMPro;

public class Login : MonoBehaviour
{
    [SerializeField] Button _loginButton;
    [SerializeField] Button _registerButton;
    [SerializeField] public GameObject FirstLoginCanvas;
    [SerializeField] InputField inputPassword;
    [SerializeField] InputField inputName;
    [SerializeField] Text passnumChr;
    [SerializeField] Text namenumChr;
    [SerializeField] Text ErrorOrSuccessText;

    [Header("Player�̖��OUI")]
    [SerializeField] TextMeshProUGUI MyNameText;

    [Header("Buttons")]
    [SerializeField] Color normalColor;
    [SerializeField] Color pushedColor;
    [SerializeField] Image _loginbutton;
    [SerializeField] Image _registerbutton;

    public static string MyNickName;
    public static bool isLoggedin;
    private void Start()
    {
        isLoggedin = false;
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
        _loginButton.enabled = true;
        _registerButton.enabled = true;
        MyNameText.text = null;
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
        else
        {
            //1�x�����Ȃ��悤�ɂ���
            _loginButton.enabled = false;
            _registerButton.enabled = false;

            _loginbutton.color = normalColor;
            _registerbutton.color = pushedColor;
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
        isLoggedin = true;
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
        else
        {
            //1�x�����Ȃ��悤�ɂ���
            _loginButton.enabled = false;
            _registerButton.enabled = false;

            _loginbutton.color = pushedColor;
            _registerbutton.color = normalColor;
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
        isLoggedin = true;
        Debug.Log("Logged In!");
        ErrorOrSuccessText.text = "���O�C�������I";
        MyNickName = result.InfoResultPayload.PlayerProfile.DisplayName;
        MyNameText.text = MyNickName;
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
            MyNameText.text = MyNickName;
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }
    void OnError(PlayFabError error)
    {
        //������悤�ɂ���
        _loginButton.enabled = true;
        _registerButton.enabled = true;

        _loginbutton.color = normalColor;
        _registerbutton.color = normalColor;

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
