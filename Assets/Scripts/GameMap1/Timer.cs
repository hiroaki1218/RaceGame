using Photon.Pun;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static void TimerCountUp()
    {
        // �܂����[���ɎQ�����Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.InRoom) { return; }
        // �܂��Q�[���̊J�n�������ݒ肳��Ă��Ȃ��ꍇ�͍X�V���Ȃ�
        if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }

        // �Q�[���̌o�ߎ��Ԃ����߂āA�������ʂ܂ŕ\������
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
