
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class ColorChange : MonoBehaviourPunCallbacks
{
    public GameObject PlayerUI;
    public static ColorChange instance;
    public Color[] PLAYER_COLOR = new Color[] { Color.white, Color.red, Color.green, Color.blue, Color.yellow };
    public int PickedColorNum;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //プレイヤーのUIは非表示
    private void Start()
    {
        PlayerUI.SetActive(false);
    }

    //色チェンジのボタン押したとき色決定
    public void OnClickColorChangeButton(int colorIndex)
    {
        PickedColorNum = colorIndex;
        var playerObject = GameObject.Find("Player");
        playerObject.transform.GetChild(3).gameObject.GetComponent<Renderer>().material.color = PLAYER_COLOR[colorIndex];
        playerObject.transform.GetChild(6).gameObject.GetComponent<Renderer>().material.color = PLAYER_COLOR[colorIndex];
    }
}
