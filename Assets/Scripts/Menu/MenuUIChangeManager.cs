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
    public static int MenuState;

    void Start()
    {
        CarColorChangeUI.SetActive(false);
        CharacterChangeUI.SetActive(false);
        ItemUI.SetActive(false);
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

        MenuState = t;
        if (t == 0)
        {
            //Play
            MatchStartButton.SetActive(true);
            MatchCancelButton.SetActive(true);
            return;
        }
        else if(t == 1)
        {
            //�L�����F�`�F���W
            CarColorChangeUI.SetActive(true);
        }
        else if (t == 2)
        {
            //�L�����`�F���W
            CharacterChangeUI.SetActive(true);
            //�L�����σJ����
            MenuCameraManager.instance.CharacterChangeCameraActive();
        }
        else if( t == 3)
        {
            //�A�C�e������
            ItemUI.SetActive(true);
            //�A�C�e���A�����L���O�̃J����
            MenuCameraManager.instance.ItemAndRankingCameraActive();
        }
    }
}
