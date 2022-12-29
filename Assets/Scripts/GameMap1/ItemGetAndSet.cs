using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGetAndSet : MonoBehaviour
{
    [SerializeField] private ItemReelController itemreelcontroller;
    [SerializeField] private Image[] itemSlot;
    [SerializeField] public Items[] itemInSlot;
    [SerializeField] private Items item;
    private int itemChangeInt;
    public int specialItemInt = 90;

    private int recognizeSpecialItemInt;
    private int recognizeNormalItemInt;

    public int ItemNumber;
    public bool isGetItemSlot1;
    public bool isGetItemSlot2;

    public bool isSpecial;
    public bool gotItemFirst;
    //総アイテム数19
    //回復:2,1 攻撃:7,1 防御:2,1 スピード:2,1 スペシャル:2

    private void Start()
    {
        //for (int i = 0; i < itemSlot.Length; i++)
        //{
            //itemSlot[i].sprite = null;
        //}

        isSpecial = false;
    }

    public void GetItem()
    {
        gotItemFirst = true;
        itemChangeInt = Random.Range(0, 100);

        if (itemChangeInt >= specialItemInt)
        {
            //スペシャルのどれか
            recognizeSpecialItemInt = Random.Range(1, 7);

            Debug.Log("スペシャル！");
            isSpecial = true;

            switch (recognizeSpecialItemInt)
            {
                case 1:
                    item.type = Items.Type.Newcar;
                    ItemNumber = 2;
                    break;
                case 2:
                    item.type = Items.Type.GuidedMissile;
                    ItemNumber = 5;
                    break;
                case 3:
                    item.type = Items.Type.SuperCushion;
                    ItemNumber = 13;
                    break;
                case 4:
                    item.type = Items.Type.SelfDriving;
                    ItemNumber = 16;
                    break;
                case 5:
                    item.type = Items.Type.CheatCode;
                    ItemNumber = 17;
                    break;
                case 6:
                    item.type = Items.Type.YearEndJumbo;
                    ItemNumber = 18;
                    break;
            }
        }
        else
        {
            //普通のアイテムのどれか
            recognizeNormalItemInt = Random.Range(1, 14);

            switch (recognizeNormalItemInt)
            {
                case 1:
                    item.type = Items.Type.Toolbox;
                    ItemNumber = 0;
                    break;
                case 2:
                    item.type = Items.Type.Recoveryoil;
                    ItemNumber = 1;
                    break;
                case 3:
                    item.type = Items.Type.RocketLauncher;
                    ItemNumber = 3;
                    break;
                case 4:
                    item.type = Items.Type.Thumbtack;
                    ItemNumber = 4;
                    break;
                case 5:
                    item.type = Items.Type.Digitalhack;
                    ItemNumber = 6;
                    break;
                case 6:
                    item.type = Items.Type.StickyBomb;
                    ItemNumber = 7;
                    break;
                case 7:
                    item.type = Items.Type.SoundCrackingSpeaker;
                    ItemNumber = 8;
                    break;
                case 8:
                    item.type = Items.Type.Smoke;
                    ItemNumber = 9;
                    break;
                case 9:
                    item.type = Items.Type.DOSAttack;
                    ItemNumber = 10;
                    break;
                case 10:
                    item.type = Items.Type.GlassShield;
                    ItemNumber = 11;
                    break;
                case 11:
                    item.type = Items.Type.TemperedGlassShield;
                    ItemNumber = 12;
                    break;
                case 12:
                    item.type = Items.Type.Nitro;
                    ItemNumber = 14;
                    break;
                case 13:
                    item.type = Items.Type.DoubleNitro;
                    ItemNumber = 15;
                    break;
            }
        }

        //最新のゲットしたアイテム
        Debug.Log(ItemNumber);
        if(itemreelcontroller.nextItemSlot == 1)
        {
            itemreelcontroller.ItemNumberSlot1 = ItemNumber;
            itemreelcontroller.satItemSlot1 = false;
            isGetItemSlot1 = true;
        }
        else if(itemreelcontroller.nextItemSlot == 2)
        {
            itemreelcontroller.ItemNumberSlot2 = ItemNumber;
            itemreelcontroller.satItemSlot2 = false;
            isGetItemSlot2 = true;
        }

        Items getitem = ItemGenerater.instance.Spawn(item.type);
        Debug.Log("GetItem is" + item.type);
        SetItem(getitem); 
    }

    public void SetItem(Items getitem)
    {
        if(itemreelcontroller.nextItemSlot == 1)
        {
            itemInSlot[0] = getitem;
        } 
        else if(itemreelcontroller.nextItemSlot == 2)
        {
            itemInSlot[1] = getitem;
        }
    }

    public void UseItem()
    {
        itemreelcontroller.UseItem();
    }
}