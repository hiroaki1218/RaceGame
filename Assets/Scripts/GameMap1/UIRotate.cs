using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIRotate : MonoBehaviourPunCallbacks
{
    public Transform mainCameraTransform;
    public GameObject[] AllUI;

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            mainCameraTransform = GameObject.Find("MyCamera").transform;
            AllUI = GameObject.FindGameObjectsWithTag("OtherUI");

            foreach (GameObject uis in AllUI)
            {
                uis.transform.LookAt(uis.transform.position + mainCameraTransform.rotation * Vector3.right,
                mainCameraTransform.rotation * Vector3.up);
            }
        }
    }
}
