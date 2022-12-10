using UnityEngine;
using System.Collections;

public class miniCameraScript : MonoBehaviour
{
    public static miniCameraScript instance;
    public Transform MiniMapTarget;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void LateUpdate()
    {
        if (MiniMapTarget == null) return;

        transform.position = new Vector3(MiniMapTarget.position.x, transform.position.y, MiniMapTarget.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, MiniMapTarget.eulerAngles.y, transform.eulerAngles.z);
    }
}