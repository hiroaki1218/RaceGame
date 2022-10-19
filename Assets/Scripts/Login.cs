using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Unity.Collections;

public class Login : MonoBehaviour
{
    [SerializeField] public GameObject FirstLoginCanvas;
    [SerializeField] InputField inputPassword;
    [SerializeField] InputField inputName;
    [SerializeField] Text passnumChr;
    [SerializeField] Text namenumChr;
    [SerializeField] Text ErrorOrSuccessText;
    public static LoginWithPlayFabRequest _request;

    public static string SessionTicket;
    public static string EntityId;

    private void Start()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
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

        var request = new RegisterPlayFabUserRequest
        {
            Password = "123456",
            Username = inputName.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);


        var Request = new LoginWithPlayFabRequest
        {
            Password = inputPassword.text,
            Username = inputName.text,
        };

        _request = Request;
    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SessionTicket = result.SessionTicket;
        EntityId = result.EntityToken.Entity.Id;
        Debug.Log("Registered And Logged In!");
        ErrorOrSuccessText.text = "登録＆ログイン成功！";
        NameEnter();
        StartCoroutine(LoginCanvasSetActiveFalse());
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
        //合ってるかどうか
        var request = new LoginWithPlayFabRequest
        {
            Password = "123456",
            Username = inputName.text,
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnError);

       _request = request;
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
        Debug.Log("Logged In!");
        ErrorOrSuccessText.text = "ログイン成功！";
        StartCoroutine(LoginCanvasSetActiveFalse());
        EntityId = result.EntityToken.Entity.Id;
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
            Debug.Log("プレイヤー名：" + result.DisplayName);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }
    void OnError(PlayFabError error)
    {
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
