using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReelController : MonoBehaviour
{
    [SerializeField] ItemGetAndSet _itemgetsndset;
    int[] positions = { 0, 130, 260, 390, 520, 650, 780, 910, 1040, 1170, 1300, 1430, 1560, 1690, 1820, 1950, 2080, 2210, 2340, 2470, 2600};

    [Header("Reel1")]
    [SerializeField] RectTransform reel1;
    [Header("‰ŠúˆÊ’u")]
    [SerializeField] Vector3 reel1transform;
    [Header("‰ñ“]‘¬“x")]
    [SerializeField] float firstRotateSpeed = 3f;
    [SerializeField] float secondRotateSpeed = 1f;

    int beforefinishFirstRotate;
    bool finishedFirstRotate;
    bool satItem;

    private void Start()
    {
        finishedFirstRotate = false;
        satItem = false;
    }

    private void Update()
    {
        if (_itemgetsndset.isGetItem)
        {
            HighSpeedRotate();
            LowSpeedRotateAndSetItem();
        }
    }

    void HighSpeedRotate()
    {
        //Å‰‚Ì‰ñ“]
        if (!finishedFirstRotate)
        {
            reel1.transform.Translate(0, firstRotateSpeed, 0);

            if (reel1.anchoredPosition.y > 2380)
            {
                reel1.anchoredPosition = reel1transform;
            }

            //‚Ç‚±‚ÅŒ¸‘¬‚·‚é‚©i‚T‚Â‘O‚Ìy‚Ì’l‚ÌŽžj
            if (_itemgetsndset.ItemNumber >= 5)
            {
                beforefinishFirstRotate = _itemgetsndset.ItemNumber - 5;
            }
            else
            {
                beforefinishFirstRotate = 18 - (4 - _itemgetsndset.ItemNumber);
            }

            //Œ¸‘¬ŠJŽn
            if (reel1.anchoredPosition.y < positions[beforefinishFirstRotate + 1] && reel1.anchoredPosition.y > positions[beforefinishFirstRotate])
            {
                finishedFirstRotate = true;
            }
        }
    }

    void LowSpeedRotateAndSetItem()
    {
        if (finishedFirstRotate)
        {
            //targetItem‚ÉŒü‚©‚Á‚ÄŒ¸‘¬
            if (reel1.anchoredPosition.y < positions[_itemgetsndset.ItemNumber + 1] && reel1.anchoredPosition.y > positions[_itemgetsndset.ItemNumber])
            {
                Debug.Log("set");
                satItem = true;

                Vector3 set = reel1.anchoredPosition;
                set.y = positions[_itemgetsndset.ItemNumber];

                reel1.anchoredPosition = set;
            }
            else
            {
                //‚ä‚Á‚­‚è‰ñ“]
                if (!satItem)
                {
                    reel1.transform.Translate(0, secondRotateSpeed, 0);

                    if (reel1.anchoredPosition.y > 2380)
                    {
                        reel1.anchoredPosition = reel1transform;
                    }
                }
            }
        }
    }
}
