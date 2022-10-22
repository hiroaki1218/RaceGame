
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ColorChange : MonoBehaviourPunCallbacks
{
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

    //色チェンジのボタン押したとき色決定
    public void OnClickColorChangeButton(int colorIndex)
    {
        PickedColorNum = colorIndex;
        var playerObject = GameObject.Find("Player");
        playerObject.transform.GetChild(3).gameObject.GetComponent<Renderer>().material.color = PLAYER_COLOR[colorIndex];
        playerObject.transform.GetChild(6).gameObject.GetComponent<Renderer>().material.color = PLAYER_COLOR[colorIndex];
    }
}
