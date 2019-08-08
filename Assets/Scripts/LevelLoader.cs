using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel = null;
    [SerializeField] Image LoadIndicator;

    public void LoadLevel(string Scene)
    {
        StartCoroutine(LoadAsynchronously(Scene));
    }

    IEnumerator LoadAsynchronously (string Scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Scene);

        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            LoadIndicator.fillAmount = progress;

            yield return null;
        }
    }
}
