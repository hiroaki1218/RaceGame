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
            StatisticName = "レースタイム"
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

                //計算
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
                RankingText[score.Position].text = $"{score.Position + 1}位:{score.DisplayName}" + " " + $"{min}分{sec}秒{dot}";
                Debug.Log("Loading...");
            }
            Debug.Log("Load completed!");
            //rankingのロードが完了した時
            StartCoroutine(RankingLoadCompleted());
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    //rankingのロードが完了したら、ロード画面を消す
    IEnumerator RankingLoadCompleted()
    {
        yield return new WaitForSeconds(2);
        RankingLoadingCanvas.SetActive(false);
    }

    //指定した桁の値を返す
    public int GetPointDigit(int num, int digit)
    {
        return (int)(num / Mathf.Pow(10, digit - 1)) % 10;
    }

    //ロビー（メニュー）に戻るボタンを押したら、シーン移動
    public void OnClickGoMenuButton()
    {
        loadingscene.LoadNextScene("Menu");
    }
}
