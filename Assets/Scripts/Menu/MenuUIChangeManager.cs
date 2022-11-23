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
        //�ʏ�J����
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
            //�L�����F�`�F���W
            CarColorChangeUI.SetActive(true);
            Car.color = pushedButtonColor;
        }
        else if (t == 2)
        {
            //�L�����`�F���W
            CharacterChangeUI.SetActive(true);
            Character.color = pushedButtonColor;
            //�L�����σJ����
            MenuCameraManager.instance.CharacterChangeCameraActive();
        }
        else if(t == 3)
        {
            //�A�C�e������
            ItemUI.SetActive(true);
            Item.color = pushedButtonColor;
            //�A�C�e���A�����L���O�̃J����
            MenuCameraManager.instance.ItemAndRankingCameraActive();
        }
        else if(t == 4)
        {
            //�����L���O
            RankingUI.SetActive(true);
            Ranking.color = pushedButtonColor;
            //�A�C�e���A�����L���O�̃J����
            MenuCameraManager.instance.ItemAndRankingCameraActive();
        }
    }
}
