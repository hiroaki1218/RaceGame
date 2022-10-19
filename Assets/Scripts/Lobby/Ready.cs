using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class Ready : MonoBehaviourPunCallbacks
{
    [SerializeField] LoadingScene loadingScene;
    [SerializeField] GameObject ReadyButton;
    [SerializeField] GameObject CancelButton;
    [SerializeField] TextMeshProUGUI CountText;
    int allReadyCount;
    bool SceneChanged;

    private void Start()
    {
        ReadyButton.SetActive(true);
        CancelButton.SetActive(false);
        SceneChanged = false;
        CountText.text = $"��������:{allReadyCount}/{PhotonNetwork.CurrentRoom.PlayerCount}";
    }

    //���������{�^�����������Ƃ�
    public void OnClickReadyButton()
    {
        photonView.RPC(nameof(ReadyBoolCountUp), RpcTarget.AllViaServer);
        ReadyButton.SetActive(false);
        StartCoroutine(CancelTrueWait());
    }

    IEnumerator CancelTrueWait()
    {
        yield return new WaitForSeconds(0.8f);
        if (!SceneChanged)
        {
            CancelButton.SetActive(true);
        }      
    }

    //�L�����Z���{�^�����������Ƃ�
    public void OnClickCancelButton()
    {
        photonView.RPC(nameof(ReadyBoolCountDown), RpcTarget.AllViaServer);
        ReadyButton.SetActive(true);
        StartCoroutine(ReadyFalseWait());
    }

    IEnumerator ReadyFalseWait()
    {
        yield return new WaitForSeconds(0.8f);
        CancelButton.SetActive(false);
    }

    //���������������Ă�l���S����������A�Q�[���V�[����
    private void Update()
    {
        if(allReadyCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if (!SceneChanged)
            {
                photonView.RPC(nameof(SceneChangeToGameScene), RpcTarget.AllViaServer); 
                SceneChanged = true;
            }
        }
    }

    //�V�[���ړ�
    IEnumerator SceneChangeProgress()
    {
        yield return new WaitForSeconds(1);
        for(int i = 2; i >= 0; i--)
        {
            CountText.text = $"�J�n�܂�:{i}";
            yield return new WaitForSeconds(1);
        }
        loadingScene.LoadNextScene("GameMap1");
    }

    [PunRPC]
    void ReadyBoolCountUp()
    {
        allReadyCount++;
        CountText.text = $"��������:{allReadyCount}/{PhotonNetwork.CurrentRoom.PlayerCount}";
        Debug.Log(allReadyCount);
    }

    [PunRPC]
    void ReadyBoolCountDown()
    {
        allReadyCount--;
        CountText.text = $"��������:{allReadyCount}/{PhotonNetwork.CurrentRoom.PlayerCount}";
        Debug.Log(allReadyCount);
    }

    [PunRPC]
    void SceneChangeToGameScene()
    {
        CancelButton.SetActive(false);
        StartCoroutine(SceneChangeProgress());
    }
}
