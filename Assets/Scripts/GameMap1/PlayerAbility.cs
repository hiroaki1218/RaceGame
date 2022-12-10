using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAbility : MonoBehaviour
{
    public KindOfCharacter myCharacter;
    [SerializeField] MyHP _myhp;
    [SerializeField] BoostController _boostController;

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
            _myhp.takeDamage = _myhp.takeDamage * 1.3f;
        }
        else if (myCharacter == KindOfCharacter.Proracer)
        {
            _boostController.maxdrifttime = 1.3f * 0.97f;
        }
        else if (myCharacter == KindOfCharacter.Bomber)
        {

        }
        else if (myCharacter == KindOfCharacter.Hacker)
        {

        }
    }
}
