
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class ColorChange : MonoBehaviourPunCallbacks
{
    public GameObject ToOtherPlayerUI;
    public static ColorChange instance;
    public Color[] PLAYER_COLOR = new Color[] { Color.red, Color.white, Color.green, Color.blue, Color.yellow, Color.magenta, Color.gray, Color.cyan};
    public int PickedColorNum;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //他プレイヤーに向けたUIは非表示
    private void Start()
    {
        ToOtherPlayerUI.SetActive(false);
    }

    //色チェンジのボタン押したとき色決定
    public void OnClickColorChangeButton(int colorIndex)
    {
        PickedColorNum = colorIndex;
        var playerObject = GameObject.Find("Player");
        GameObject parent = playerObject.transform.Find("Car").gameObject;
        parent.transform.GetChild(3).gameObject.GetComponent<Renderer>().material.color = PLAYER_COLOR[colorIndex];
        parent.transform.GetChild(6).gameObject.GetComponent<Renderer>().material.color = PLAYER_COLOR[colorIndex];
    }
}
