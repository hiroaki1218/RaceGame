using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAbility : MonoBehaviour
{
    public KindOfCharacter myCharacter;

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
            MyHP.increaseHP = 4 * 1.3f;
        }
        else if(myCharacter == KindOfCharacter.Gambler)
        {

        }
        else if (myCharacter == KindOfCharacter.Proracer)
        {

        }
        else if (myCharacter == KindOfCharacter.Bomber)
        {

        }
        else if (myCharacter == KindOfCharacter.Hacker)
        {

        }
    }
}
