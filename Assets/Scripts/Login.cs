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

    [Header("Playerの名前UI")]
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
    //登録
    public void RegisterButton()
    {
        //UserName
        if (inputName.text.Length <= 2)
        {
            namenumChr.color = new Color(1.0f, 0f, 0f, 1.0f);
            Debug.Log("Nameの文字数が正しくありません。");
            StartCoroutine(ColorBack());
            return;
        }
        else
        {
            //1度押せないようにする
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
        ErrorOrSuccessText.text = "登録＆ログイン成功！";
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
            Debug.Log("Nameの文字数が正しくありません。");
            StartCoroutine(ColorBack());
            return;
        }
        else
        {
            //1度押せないようにする
            _loginButton.enabled = false;
            _registerButton.enabled = false;

            _loginbutton.color = pushedColor;
            _registerbutton.color = normalColor;
        }
        //合ってるかどうか
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
        
    //色が戻るまで数秒まつ
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
        ErrorOrSuccessText.text = "ログイン成功！";
        MyNickName = result.InfoResultPayload.PlayerProfile.DisplayName;
        MyNameText.text = MyNickName;
        Debug.Log("MyNickName is" + MyNickName);
        StartCoroutine(LoginCanvasSetActiveFalse());
    }
    //ログイン画面消すまで数秒まつ
    IEnumerator LoginCanvasSetActiveFalse()
    {
        yield return new WaitForSeconds(2);
        FirstLoginCanvas.SetActive(false);
    }

    //入力毎に監視
    public void LineCount()
    {
        //改行防止
        if (inputName.text.IndexOf("\n") == -1)
        {
            return;
        }
        inputName.text = inputName.text.Trim();
    }

    //完了ボタン
    public void NameEnter()
    {
        //名前の登録
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = inputName.text
        }, result =>
        {
            MyNickName = result.DisplayName;
            Debug.Log("MyNickName is：" + result.DisplayName);
            MyNameText.text = MyNickName;
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }
    void OnError(PlayFabError error)
    {
        //押せるようにする
        _loginButton.enabled = true;
        _registerButton.enabled = true;

        _loginbutton.color = normalColor;
        _registerbutton.color = normalColor;

        Debug.Log(error.GenerateErrorReport());
        if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            ErrorOrSuccessText.text = "このユーザーネームは既に使われています。";
        }
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            ErrorOrSuccessText.text = "ユーザーネームが間違っています。";
        }
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            ErrorOrSuccessText.text = "ユーザーネームが間違っています。";
        }
    }
}
