using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemReelController : MonoBehaviour
{
    [SerializeField] ItemGetAndSet _itemgetsndset;
    int[] positions = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000, 16000, 17000, 18000, 19000, 20000};
    int changeINT = 18700;

    [Header("Reel")]
    [SerializeField] GameObject reel1Image;
    [SerializeField] GameObject reel2Image;
    [SerializeField] RectTransform[] reel;

    [Header("�����ʒu")]
    [SerializeField] Vector3 reel1transform;
    [SerializeField] Vector3 reel2transform;
    [Header("��]���x")]
    [SerializeField] float firstRotateSpeed = 6f;
    [SerializeField] float secondRotateSpeed = 4f;

    public int ItemNumberSlot1;
    public int ItemNumberSlot2;
    int beforefinishFirstRotateSlot1;
    int beforefinishFirstRotateSlot2;
    float timeSlot1;
    float timeSlot2;
    bool finishedFirstRotateSlot1;
    bool finishedFirstRotateSlot2;
    public int nextItemSlot;
    public bool satItemSlot1;
    public bool satItemSlot2;

    private void Start()
    {
        reel1Image.SetActive(false);
        reel2Image.SetActive(false);
        finishedFirstRotateSlot1 = false;
        finishedFirstRotateSlot2 = false;
        nextItemSlot = 1;
        satItemSlot1 = false;
        satItemSlot2 = false;
    }

    private void Update()
    {
        if (_itemgetsndset.isGetItemSlot1)
        {
            if (!satItemSlot1)
            {
                nextItemSlot = 2;
                reel1Image.SetActive(true);

                HighSpeedRotateFirst(0);
                LowSpeedRotateAndSetItemFirst(0);
            }
            else
            {
                timeSlot1 = 0;
            }
        }

        if(_itemgetsndset.isGetItemSlot2)
        {
            if (!satItemSlot2)
            {
                nextItemSlot = 1;
                reel2Image.SetActive(true);
                HighSpeedRotateSecond(1);
                LowSpeedRotateAndSetItemSecond(1);
            }
            else
            {
                timeSlot2 = 0;
            }
        }
    }

    void HighSpeedRotateFirst(int reelNum)
    {
        //�ŏ��̉�]
        if (!finishedFirstRotateSlot1)
        {
            reel[reelNum].transform.Translate(0, firstRotateSpeed, 0);

            if (reel[reelNum].anchoredPosition.y > changeINT)
            {
                reel[reelNum].anchoredPosition = reel1transform;
            }

            //�ǂ��Ō������邩�i�T�O��y�̒l�̎��j
            if (_itemgetsndset.ItemNumber >= 5)
            {
                beforefinishFirstRotateSlot1 = _itemgetsndset.ItemNumber - 5;
            }
            else
            {
                beforefinishFirstRotateSlot1 = 18 - (4 - _itemgetsndset.ItemNumber);
            }

            timeSlot1 += Time.deltaTime;
            if(timeSlot1 > 2)
            {
                //�����J�n
                if (reel[reelNum].anchoredPosition.y < positions[beforefinishFirstRotateSlot1 + 1] && reel[reelNum].anchoredPosition.y > positions[beforefinishFirstRotateSlot1])
                {
                    finishedFirstRotateSlot1 = true;
                }
            }
        }
    }

    void LowSpeedRotateAndSetItemFirst(int reelNum)
    {
        if (finishedFirstRotateSlot1)
        {
            //targetItem�Ɍ������Č���
            if (reel[reelNum].anchoredPosition.y < positions[ItemNumberSlot1 + 1] && reel[reelNum].anchoredPosition.y > positions[ItemNumberSlot1])
            {
                satItemSlot1 = true;

                Vector3 set = reel[reelNum].anchoredPosition;
                set.y = positions[ItemNumberSlot1];

                reel[reelNum].anchoredPosition = set;
            }
            else
            {
                //��������]
                if (!satItemSlot1)
                {
                    reel[reelNum].transform.Translate(0, secondRotateSpeed, 0);

                    if (reel[reelNum].anchoredPosition.y > changeINT)
                    {
                        reel[reelNum].anchoredPosition = reel1transform;
                    }
                }
            }
        }
    }


    void HighSpeedRotateSecond(int reelNum)
    {
        //�ŏ��̉�]
        if (!finishedFirstRotateSlot2)
        {
            reel[reelNum].transform.Translate(0, firstRotateSpeed, 0);

            if (reel[reelNum].anchoredPosition.y > changeINT)
            {
                reel[reelNum].anchoredPosition = reel2transform;
            }

            //�ǂ��Ō������邩�i�T�O��y�̒l�̎��j
            if (_itemgetsndset.ItemNumber >= 5)
            {
                beforefinishFirstRotateSlot2 = _itemgetsndset.ItemNumber - 5;
            }
            else
            {
                beforefinishFirstRotateSlot2 = 18 - (4 - _itemgetsndset.ItemNumber);
            }

            timeSlot2 += Time.deltaTime;

            if(timeSlot2 > 2.8f)
            {
                //�����J�n
                if (reel[reelNum].anchoredPosition.y < positions[beforefinishFirstRotateSlot2 + 1] && reel[reelNum].anchoredPosition.y > positions[beforefinishFirstRotateSlot2])
                {
                    finishedFirstRotateSlot2 = true;
                }
            } 
        }
    }

    void LowSpeedRotateAndSetItemSecond(int reelNum)
    {
        if (finishedFirstRotateSlot2)
        {
            //targetItem�Ɍ������Č���
            if (reel[reelNum].anchoredPosition.y < positions[ItemNumberSlot2 + 1] && reel[reelNum].anchoredPosition.y > positions[ItemNumberSlot2])
            {

                Vector3 set = reel[reelNum].anchoredPosition;
                set.y = positions[ItemNumberSlot2];

                reel[reelNum].anchoredPosition = set;

                satItemSlot2 = true;
            }
            else
            {
                if (!satItemSlot2)
                {
                    reel[reelNum].transform.Translate(0, secondRotateSpeed, 0);

                    if (reel[reelNum].anchoredPosition.y > changeINT)
                    {
                        reel[reelNum].anchoredPosition = reel2transform;
                    }
                }  
            }
        }
    }

    public void UseItem()
    {
        if (CanUseItem())
        {
            //�o�[�`�����A�C�e������
            Debug.Log($"�g�����A�C�e���́F{_itemgetsndset.itemInSlot[0].type}");
            _itemgetsndset.itemInSlot[0] = null;

            reel1Image.SetActive(false);
            finishedFirstRotateSlot1 = false;
            finishedFirstRotateSlot2 = false;

            _itemgetsndset.isGetItemSlot1 = false;
            satItemSlot1 = false;

            //2�Ԗڂ̃X���b�g�ɃA�C�e��������Ƃ�
            if (satItemSlot2)
            {
                _itemgetsndset.itemInSlot[0] = _itemgetsndset.itemInSlot[1];
                _itemgetsndset.itemInSlot[1] = null;
                Debug.Log("2�Ԃ���");
                //2�Ԗڂ̃��[���̏������Ȃ���
                _itemgetsndset.isGetItemSlot2 = false;
                nextItemSlot = 2;
                satItemSlot1 = true;
                satItemSlot2 = false;
                reel1Image.SetActive(true);
                reel2Image.SetActive(false);
                //2�Ԗڂ̃��[���̍����܂Œ���
                Vector3 set = reel[0].anchoredPosition;
                set.y = positions[ItemNumberSlot2];

                reel[0].anchoredPosition = set;
            }
            else if (nextItemSlot == 1)
            {
                _itemgetsndset.itemInSlot[0] = _itemgetsndset.itemInSlot[1];
                _itemgetsndset.itemInSlot[1] = null;
                Debug.Log("2�ԓr��");
                //2�Ԗڂ̃��[���̏������Ȃ���
                _itemgetsndset.isGetItemSlot2 = false;
                nextItemSlot = 2;
                satItemSlot1 = true;
                satItemSlot2 = false;
                reel1Image.SetActive(true);
                reel2Image.SetActive(false);
                //2�Ԗڂ̃��[���̍����܂Œ���
                int i = ItemNumberSlot2;
                Vector3 set = reel[0].anchoredPosition;
                set.y = positions[i];

                reel[0].anchoredPosition = set;
            }
            else
            {
                nextItemSlot = 1;
            }
        }
    }

    bool CanUseItem()
    {
        if (satItemSlot1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
