using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MyHP : MonoBehaviourPunCallbacks, IPunObservable
{
    private const float MaxHP = 100f;
    public float increaseHP = 4;

    [SerializeField] public Slider myHPBar = default;
    [SerializeField] public Slider toOtherHPBar = default;
    private float currentHP = MaxHP;
    public float takeDamage;

    PhotonView myPV;
    bool IsGetComponents;
    public bool isMyPlayerAvatar;

    //Componentの取得
    public void GetComponentsHP()
    {
        IsGetComponents = false;
        myPV = GetComponent<PhotonView>();
        myHPBar = transform.Find("MyUI/HPAndShieldCanvas/Shield/ShieldBar/HP/HPBar").gameObject.GetComponent<Slider>();
        toOtherHPBar = transform.Find("toOtherUI/HPAndShieldCanvas/Shield/ShieldBar/HP/HPBar").gameObject.GetComponent<Slider>();
        IsGetComponents = true;
    }

    private void Update()
    {
        Mathf.Clamp(currentHP, 0, 100);
        if (!IsGetComponents) return;

        if (myPV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                takeDamage = 30;
            }

            if (takeDamage > 0)
            {
                // 入力があったら、スタミナを減少させる
                currentHP = currentHP - takeDamage;
                if(currentHP <= 0)
                {
                    currentHP = 0;
                }
                takeDamage = 0;
            }
            else
            {
                // 入力がなかったら、スタミナを回復させる
                currentHP = currentHP + Time.deltaTime * increaseHP;
                if (currentHP >= MaxHP)
                {
                    currentHP = MaxHP;
                }
            }
           
        }

        // スタミナをゲージに反映する
        myHPBar.value = currentHP / MaxHP;
        toOtherHPBar.value = currentHP / MaxHP;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 自身のアバターのHPを送信する
            stream.SendNext(currentHP);
        }
        else
        {
            // 他プレイヤーのアバターのHPを受信する
            currentHP = (float)stream.ReceiveNext();
        }
    }
}
