using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraManager : MonoBehaviour
{
    //�ʏ�̎��ɂق��̃J�����؂�Y�꒍�ӁI
    [SerializeField] GameObject MainCamera;
    [SerializeField] GameObject CharacterChangeCamera;
    [SerializeField] GameObject ItemAndRankCamera;
    public static MenuCameraManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        MainCamera.SetActive(true);
        CharacterChangeCamera.SetActive(false);
        ItemAndRankCamera.SetActive(false);
    }

    //�L�����ς̂Ƃ��̂݃L�����σJ�����A�N�e�B�u
    public void CharacterChangeCameraActive()
    {
        MainCamera.SetActive(false);
        CharacterChangeCamera.SetActive(true);
    }

    //�ʏ펞
    public void MainCameraActive()
    {
        MainCamera.SetActive(true);
        CharacterChangeCamera.SetActive(false);
        ItemAndRankCamera.SetActive(false);
    }

    //�A�C�e�������ƃ����L���O�\���̎��̂�
    public void ItemAndRankingCameraActive()
    {
        MainCamera.SetActive(false);
        ItemAndRankCamera.SetActive(true);
    }
}
