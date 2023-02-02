using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingScreenScript : MonoBehaviour
{
    private ProgressBar progressBar;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        progressBar = root.Q<ProgressBar>("ProgressBar");
    }

    public void LoadScene(string SceneName)
    {
        StartCoroutine(LoadSceneAsync(SceneName));
    }

    private IEnumerator LoadSceneAsync(string SceneName)
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneName);

        loadScene.allowSceneActivation = false;

        while (!loadScene.isDone)
        {
            progressBar.value = Mathf.Clamp01(loadScene.progress / 0.9f);

            if (loadScene.progress >= 0.9f)
                loadScene.allowSceneActivation = true;

            yield return null;
        }
    }
}
