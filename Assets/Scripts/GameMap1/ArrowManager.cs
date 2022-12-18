using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager instance;

    [SerializeField] public TextMeshProUGUI[] targetplayertext;
    [SerializeField] public string[] localplayernumber;
    private bool once;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        once = true;
    }

    private void FixedUpdate()
    {
        if (SpawnSystem.instance.isAllSpawned)
        {
            for (int i = 0; i < targetplayertext.Length; i++)
            {
                if (targetplayertext[i] == null) break;
                //l”•ªl‚»‚ê‚¼‚ê”Ô†‚ð‚Â‚¯‚é
                targetplayertext[i].text = localplayernumber[i];
            }
        }
    }
}
