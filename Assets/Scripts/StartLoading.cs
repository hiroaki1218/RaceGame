using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoading : MonoBehaviour
{
    [SerializeField] GameObject StartLoadCanvas;

    private void Awake()
    {
        StartLoadCanvas.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(WaitTillStart());
    }

    IEnumerator WaitTillStart()
    {
        yield return new WaitForSeconds(2);
        StartLoadCanvas.SetActive(false);
    }
}
