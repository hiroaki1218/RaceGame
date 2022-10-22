using Photon.Pun;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static void TimerCountUp()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }
        // まだゲームの開始時刻が設定されていない場合は更新しない
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

        // ゲームの経過時間を求めて、小数第一位まで表示する
        float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);

        GameMap1Controller.instance.sec = elapsedTime;
        GameMap1Controller.instance.timersec = (int)(GameMap1Controller.instance.sec * 100);
        if (GameMap1Controller.instance.sec >= 60)
        {
            GameMap1Controller.instance.timermin++;
            GameMap1Controller.instance.sec = 0;
        }
    }
}
