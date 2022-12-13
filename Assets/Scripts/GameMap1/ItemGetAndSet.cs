using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGetAndSet : MonoBehaviour
{
    [SerializeField] private Image[] itemSlot;
    [SerializeField] private Items.Type[] itemtypeInSlot;
    [SerializeField] private Items item;
    private int itemChangeInt;
    private int specialItemInt = 91;

    private int recognizeSpecialItemInt;
    private int recognizeNormalItemInt;
    //���A�C�e����19
    //��:2,1 �U��:7,1 �h��:2,1 �X�s�[�h:2,1 �X�y�V����:2

    public void GetItem()
    {
        itemChangeInt = Random.Range(0, 100);

        if(itemChangeInt >= specialItemInt)
        {
            //�X�y�V�����̂ǂꂩ
            recognizeSpecialItemInt = Random.Range(1, 7);

            switch (recognizeSpecialItemInt)
            {
                case 1:
                    item.type = Items.Type.Newcar;
                    break;
                case 2:
                    item.type = Items.Type.GuidedMissile;
                    break;
                case 3:
                    item.type = Items.Type.SuperCushion;
                    break;
                case 4:
                    item.type = Items.Type.SelfDriving;
                    break;
                case 5:
                    item.type = Items.Type.CheatCode;
                    break;
                case 6:
                    item.type = Items.Type.YearEndJumbo;
                    break;
            }
        }
        else
        {
            //���ʂ̃A�C�e���̂ǂꂩ
            recognizeNormalItemInt = Random.Range(1, 14);
            switch (recognizeNormalItemInt)
            {
                case 1:
                    item.type = Items.Type.Toolbox;
                    break;
                case 2:
                    item.type = Items.Type.Recoveryoil;
                    break;
                case 3:
                    item.type = Items.Type.RocketLauncher;
                    break;
                case 4:
                    item.type = Items.Type.Thumbtack;
                    break;
                case 5:
                    item.type = Items.Type.Digitalhack;
                    break;
                case 6:
                    item.type = Items.Type.StickyBomb;
                    break;
                case 7:
                    item.type = Items.Type.SoundCrackingSpeaker;
                    break;
                case 8:
                    item.type = Items.Type.Smoke;
                    break;
                case 9:
                    item.type = Items.Type.DOSAttack;
                    break;
                case 10:
                    item.type = Items.Type.GlassShield;
                    break;
                case 11:
                    item.type = Items.Type.TemperedGlassShield;
                    break;
                case 12:
                    item.type = Items.Type.Nitro;
                    break;
                case 13:
                    item.type = Items.Type.DoubleNitro;
                    break;
            }
        }

        item = ItemGenerater.instance.Spawn(item.type);
        
        SetItem(item);
    }

    public void SetItem(Items getitem)
    {
        itemSlot[0].sprite = getitem.sprite;
        itemtypeInSlot[0] = getitem.type;
    }
}
