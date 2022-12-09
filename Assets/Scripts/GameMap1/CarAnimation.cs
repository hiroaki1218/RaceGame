using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarAnimation : MonoBehaviour
{
    [SerializeField] Animator _carAnimator;
    [SerializeField] Rigidbody _rigidBody;
    public MyPlayerController myPlayerController;
    PhotonView myPV;

    private bool isMainScene;
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "GameMap1")
        {
            isMainScene = true;
        }
        else
        {
            isMainScene = false;
        }

        myPV = GetComponent<PhotonView>();
        myPlayerController = GetComponent<MyPlayerController>();
    }

    private void Update()
    {
        if (myPV.IsMine)
        {
            if (!isMainScene)
            {
                if (myPlayerController.onGround)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        _rigidBody.AddForce(new Vector3(0, 5500, 0), ForceMode.Impulse);
                    }
                }
            }
            else
            {
                if (GameMap1Controller.isStartedMatch)
                {
                    if (myPlayerController.onGround)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            _rigidBody.AddForce(new Vector3(0, 5500, 0), ForceMode.Impulse);
                        }
                    }
                }
            }
        } 
    }
}
