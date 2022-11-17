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

    bool isfirstGot;
    bool getting;

    float timer;
    int timersec;
    private void Start()
    {
        isfirstGot = false;
        getting = false;
        if (SceneManager.GetActiveScene().name == "Ranking")
        {
            GetLeaderboard();
        }
    }

    //Menuシーンのみ、ログイン後にランキングを取得
    private void Update()
    {
        if (Login.isLoggedin)
        {
            if (!isfirstGot)
            {
                GetLeaderboard();
                isfirstGot = true;
            }
            CountUp();
        }    
    }

    //10秒ごとにランキングを取得
    void CountUp()
    {
        timer += Time.deltaTime;
        timersec = (int)timer;
        if(timersec >= 10)
        {
            timer = 0;
            if (!getting)
            {
                Debug.Log("get");
                GetLeaderboard();
                Debug.Log("a");
                getting = true;
            }
        }
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
            }
            //rankingのロードが完了した時
            StartCoroutine(RankingLoadCompleted());
            getting = false;
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
