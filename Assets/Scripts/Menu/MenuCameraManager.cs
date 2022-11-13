using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraManager : MonoBehaviour
{
    //通常の時にほかのカメラ切り忘れ注意！
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

    //キャラ変のときのみキャラ変カメラアクティブ
    public void CharacterChangeCameraActive()
    {
        MainCamera.SetActive(false);
        CharacterChangeCamera.SetActive(true);
    }

    //通常時
    public void MainCameraActive()
    {
        MainCamera.SetActive(true);
        CharacterChangeCamera.SetActive(false);
        ItemAndRankCamera.SetActive(false);
    }

    //アイテム説明とランキング表示の時のみ
    public void ItemAndRankingCameraActive()
    {
        MainCamera.SetActive(false);
        ItemAndRankCamera.SetActive(true);
    }
}
