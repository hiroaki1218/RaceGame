using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class Ranking : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] RankingText;
    [SerializeField] GameObject RankingLoadingCanvas;
    [SerializeField] LoadingScene loadingscene;
    int totalscore;
    int onedigit;
    int twodigit;
    int threedigit;
    int fourdigit;
    int fivedigit;

    int min;
    int sec;
    int dot;
    private void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "���[�X�^�C��"
        }, result =>
        {
            foreach (var score in result.Leaderboard)
            {
                totalscore = -1 * score.StatValue;
                Debug.Log(totalscore);
                for (int i = 1; i <= 5; i++)
                {
                    if(i == 1)
                    {
                        onedigit = GetPointDigit(totalscore, 1);
                    }
                    if (i == 2)
                    {
                        twodigit = GetPointDigit(totalscore, 2);
                    }
                    if (i == 3)
                    {
                        threedigit = GetPointDigit(totalscore, 3);
                    }
                    if (i == 4)
                    {
                        fourdigit = GetPointDigit(totalscore, 4);
                    }
                    if (i == 5)
                    {
                        fivedigit = GetPointDigit(totalscore, 5);
                    }     
                }

                //�v�Z
                if(totalscore < 6000)
                {
                    min = 0;
                    sec = fourdigit * 10 + threedigit;
                    dot = twodigit * 10 + onedigit;
                }
                else
                {
                    min = Mathf.FloorToInt(((fivedigit * 10) + fourdigit) / 6);
                    sec = (fivedigit * 100 + fourdigit * 10 + threedigit) - (min * 60);
                    dot = twodigit * 10 + onedigit;
                }
                RankingText[score.Position].text = $"{score.Position + 1}��:{score.DisplayName}" + " " + $"{min}��{sec}�b{dot}";
                Debug.Log("Loading...");
            }
            Debug.Log("Load completed!");
            //ranking�̃��[�h������������
            StartCoroutine(RankingLoadCompleted());
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    //ranking�̃��[�h������������A���[�h��ʂ�����
    IEnumerator RankingLoadCompleted()
    {
        yield return new WaitForSeconds(2);
        RankingLoadingCanvas.SetActive(false);
    }

    //�w�肵�����̒l��Ԃ�
    public int GetPointDigit(int num, int digit)
    {
        return (int)(num / Mathf.Pow(10, digit - 1)) % 10;
    }

    //���r�[�i���j���[�j�ɖ߂�{�^������������A�V�[���ړ�
    public void OnClickGoMenuButton()
    {
        loadingscene.LoadNextScene("Menu");
    }
}
