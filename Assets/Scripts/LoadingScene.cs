using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private Slider _slider;

    private void Start()
    {
        _loadingUI.SetActive(false);
    }

    public void LoadNextScene(string SceneName)
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene(SceneName));
    }

    IEnumerator LoadScene(string SceneName)
    {
        yield return new WaitForSeconds(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            _slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1);
                _slider.value = 1;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
