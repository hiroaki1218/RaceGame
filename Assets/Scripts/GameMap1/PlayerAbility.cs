using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAbility : MonoBehaviour
{
    public KindOfCharacter myCharacter;
    [SerializeField] MyHP _myhp;
    [SerializeField] BoostController _boostController;
    [SerializeField] MyAudioController _audiocontroller;
    [SerializeField] ItemGetAndSet _itemgetandset;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Ranking") return;
        myCharacter = CharacterRoleChange.instance.pickedCharacter;
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Ranking") return;

        if(myCharacter == KindOfCharacter.Engineer)
        {
            _myhp.increaseHP = 4 * 1.3f;
        }
        else if(myCharacter == KindOfCharacter.Gambler)
        {
            _audiocontroller.isGambler = true;
            _itemgetandset.specialItemInt = 85;
            _myhp.receiveDamage = _myhp.receiveDamage * 1.3f;
        }
        else if (myCharacter == KindOfCharacter.Proracer)
        {
            _boostController.maxdrifttime = 1.3f * 0.97f;
        }
        else if (myCharacter == KindOfCharacter.Bomber)
        {
            //与えるダメージアップ
            //攻撃系のアイテムに普通のアイテムとは別のタグをつけて生成
        }
        else if (myCharacter == KindOfCharacter.Hacker)
        {
            //MiniMapにアイテム表示
        }
    }
}
