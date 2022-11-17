using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum KindOfCharacter
{ 
    Engineer,
    Gambler,
    Proracer,
    Bomber,
    Hacker,
}

public class CharacterRoleChange : MonoBehaviour
{
    public KindOfCharacter pickedCharacter;

    [SerializeField] GameObject Characters;

    //UI
    [SerializeField] TextMeshProUGUI CharacterNameText;
    [SerializeField] TextMeshProUGUI CharacterExplantiorText;

    [SerializeField] GameObject Engineer;
    [SerializeField] GameObject Gambler;
    [SerializeField] GameObject Proracer;
    [SerializeField] GameObject Bomber;
    [SerializeField] GameObject Hacker;

    public int state;
    Animator CharacterChangeAnime;
    bool canChange;

    public static CharacterRoleChange instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        CharacterChangeAnime = Characters.GetComponent<Animator>();
        canChange = true;
        state = 0;

        CharacterNameText.text = null;
        CharacterExplantiorText.text = null; 

        Engineer.SetActive(true);
        Gambler.SetActive(false);
        Proracer.SetActive(false);
        Bomber.SetActive(false);
        Hacker.SetActive(false);
    }

    //キャラチェンジ
    public void OnClickCharaChanUpButton()
    {
        if (canChange)
        {
            CharacterChangeAnime.Play("CharacterChangeUp", 0, 0);
            canChange = false;
            StartCoroutine(WaitForChange());
            state--;
            if (state <= -1)
            {
                state = 4;
            }
        }
    }

    //キャラチェンジ
    public void OnClickCharaChanDpwnButton()
    {
        if (canChange)
        {
            CharacterChangeAnime.Play("CharacterChangeDown", 0, 0);
            canChange = false;
            StartCoroutine(WaitForChange());
            state++;
            if (state == 5)
            {
                state = 0;
            }
        }
    }

    private void Update()
    {
        //stateによってキャラ名とキャラ説明文変更
        if(state == 0)
        {
            //enginner
            CharacterNameText.text = "エンジニア";
            CharacterExplantiorText.text = "aaa";
        }
        else if (state == 1)
        {
            //gambler
            CharacterNameText.text = "ギャンブラー";
            CharacterExplantiorText.text = "iii";
        }
        else if (state == 2)
        {
            //Proracer
            CharacterNameText.text = "プロレーサー";
            CharacterExplantiorText.text = "uuu";
        }
        else if (state == 3)
        {
            //Bomber
            CharacterNameText.text = "爆弾魔";
            CharacterExplantiorText.text = "eee";
        }
        else if (state == 4)
        {
            //Hacker
            CharacterNameText.text = "ハッカー";
            CharacterExplantiorText.text = "ooo";
        }
    }

    //キャラピック
    public void CharacterPickUp()
    {
        //まずすべて消す
        Engineer.SetActive(false);
        Gambler.SetActive(false);
        Proracer.SetActive(false);
        Bomber.SetActive(false);
        Hacker.SetActive(false);

        switch (state)
        {
            case 0:
                pickedCharacter = KindOfCharacter.Engineer;
                Engineer.SetActive(true);
                break;
            case 1:
                pickedCharacter = KindOfCharacter.Gambler;
                Gambler.SetActive(true);
                break;
            case 2:
                pickedCharacter = KindOfCharacter.Proracer;
                Proracer.SetActive(true);
                break;
            case 3:
                pickedCharacter = KindOfCharacter.Bomber;
                Bomber.SetActive(true);
                break;
            case 4:
                pickedCharacter = KindOfCharacter.Hacker;
                Hacker.SetActive(true);
                break;
        }
    }

    IEnumerator WaitForChange()
    {
        if (!canChange)
        {
            yield return new WaitForSeconds(0.8f);
            canChange = true;
        }
    }
}
