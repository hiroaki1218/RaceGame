using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ColorChange : MonoBehaviourPunCallbacks
{
    public static ColorChange instance;
    public GameObject Car;
    public Renderer CarRenderer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void OnClickColorChangeButton()
    {
        CarRenderer.material.color = Color.blue;
    }
}
