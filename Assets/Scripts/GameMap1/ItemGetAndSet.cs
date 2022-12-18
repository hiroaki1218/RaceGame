using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGetAndSet : MonoBehaviour
{
    [SerializeField] private Image[] itemSlot;
    [SerializeField] public Items[] itemInSlot;
    [SerializeField] private Items item;
    private int itemChangeInt;
    private int specialItemInt = 91;

    private int recognizeSpecialItemInt;
    private int recognizeNormalItemInt;
    //総アイテム数19
    //回復:2,1 攻撃:7,1 防御:2,1 スピード:2,1 スペシャル:2

    private void Start()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].sprite = null;
        }
    }

    public void GetItem()
    {
        itemChangeInt = Random.Range(0, 100);

        if (itemChangeInt >= specialItemInt)
        {
            //スペシャルのどれか
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
            //普通のアイテムのどれか
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

        Items getitem = ItemGenerater.instance.Spawn(item.type);
        Debug.Log("GetItem is" + item.type);
        SetItem(getitem);
    }

    public void SetItem(Items getitem)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (IsEmpty(i))
            {
                itemInSlot[i] = getitem;
                itemSlot[i].sprite = itemInSlot[i].sprite;

                break;
            }
        }
    }

    public bool CanUseItem()
    {
        if (itemSlot[0].sprite != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsEmpty(int i)
    {
        if (itemSlot[i].sprite == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseItem()
    {
        if (CanUseItem())
        {
            Debug.Log(itemInSlot[0].type);
            RemoveItem(0);
            MoveItem();
        }
    }

    private void RemoveItem(int i)
    {
        itemSlot[i].sprite = null;
    }

    private void MoveItem()
    {
        if (itemSlot[1].sprite != null)
        {
            Items moveitem = itemInSlot[1];
            RemoveItem(1);
            SetItem(moveitem);
        }
    }
}