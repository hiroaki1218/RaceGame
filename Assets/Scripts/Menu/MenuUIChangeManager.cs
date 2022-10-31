using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIChangeManager : MonoBehaviour
{
    [SerializeField] GameObject CarColorChangeUI;
    [SerializeField] GameObject CharacterChangeUI;

    void Start()
    {
        CarColorChangeUI.SetActive(false);
        CharacterChangeUI.SetActive(false);
    }

    public void OnClickMunuChangeButton(int t)
    {
        CarColorChangeUI.SetActive(false);
        CharacterChangeUI.SetActive(false);

        if (t == 0)
        {
            return;
        }
        else if(t == 1)
        {
            CarColorChangeUI.SetActive(true);
        }
        else if (t == 2)
        {
            CharacterChangeUI.SetActive(true);
        }
    }
}
