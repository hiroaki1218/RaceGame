using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetPosition : MonoBehaviour
{
    [SerializeField] GameObject thisItem;
    [SerializeField] Items.Type type;
    [SerializeField] ItemGetAndSet _itemGetAndSet;

    private void FixedUpdate()
    {
        if (_itemGetAndSet.CanUseItem())
        {
            if(type == _itemGetAndSet.itemInSlot[0].type)
            {
                thisItem.SetActive(true);
            }
            else
            {
                thisItem.SetActive(false);
            }
        }
        else
        {
            thisItem.SetActive(false);
        }
    }
}
