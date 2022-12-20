using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBigJemp : MonoBehaviour
{
    public Rigidbody targetPlayer;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            targetPlayer = other.GetComponent<Rigidbody>();
            targetPlayer.AddForce(new Vector3(0, 40000, 0), ForceMode.Impulse);
        } 
    }
}
