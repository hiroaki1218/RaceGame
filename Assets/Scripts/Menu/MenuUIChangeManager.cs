using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIChangeManager : MonoBehaviour
{
    //MatchStartButton
    [SerializeField] GameObject MatchStartButton;
    [SerializeField] GameObject MatchCancelButton;

    [SerializeField] GameObject CarColorChangeUI;
    [SerializeField] GameObject CharacterChangeUI;
    [SerializeField] GameObject ItemUI;
    [SerializeField] GameObject RankingUI;

    [Header("Buttons")]
    [SerializeField] Color pushedButtonColor;
    [SerializeField] Color normalColor;
    [SerializeField] Image Play;
    [SerializeField] Image Car;
    [SerializeField] Image Character;
    [SerializeField] Image Item;
    [SerializeField] Image Ranking;

    public static int MenuState;

    void Start()
    {
        CarColorChangeUI.SetActive(false);
        CharacterChangeUI.SetActive(false);
        ItemUI.SetActive(false);
        RankingUI.SetActive(false);
        Play.color = pushedButtonColor;
    }

    public void OnClickMunuChangeButton(int t)
    {
        //通常カメラ
        MenuCameraManager.instance.MainCameraActive();

        MatchStartButton.SetActive(false);
        MatchCancelButton.SetActive(false);

        CarColorChangeUI.SetActive(false);
        CharacterChangeUI.SetActive(false);
        ItemUI.SetActive(false);
        RankingUI.SetActive(false);

        Play.color = normalColor;
        Car.color = normalColor;
        Character.color = normalColor;
        Item.color = normalColor;
        Ranking.color = normalColor;

        MenuState = t;
        if (t == 0)
        {
            //Play
            MatchStartButton.SetActive(true);
            MatchCancelButton.SetActive(true);
            Play.color = pushedButtonColor;
            return;
        }
        else if(t == 1)
        {
            //キャラ色チェンジ
            CarColorChangeUI.SetActive(true);
            Car.color = pushedButtonColor;
        }
        else if (t == 2)
        {
            //キャラチェンジ
            CharacterChangeUI.SetActive(true);
            Character.color = pushedButtonColor;
            //キャラ変カメラ
            MenuCameraManager.instance.CharacterChangeCameraActive();
        }
        else if(t == 3)
        {
            //アイテム説明
            ItemUI.SetActive(true);
            Item.color = pushedButtonColor;
            //アイテム、ランキングのカメラ
            MenuCameraManager.instance.ItemAndRankingCameraActive();
        }
        else if(t == 4)
        {
            //ランキング
            RankingUI.SetActive(true);
            Ranking.color = pushedButtonColor;
            //アイテム、ランキングのカメラ
            MenuCameraManager.instance.ItemAndRankingCameraActive();
        }
    }
}
