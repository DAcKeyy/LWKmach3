using LWT.System;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace LWT.System
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject loadingPanel;

        [SerializeField]
        private Image loadIndicator;

        [Inject]
        private InputHandles inputHandles;

        private Action loadScene;

        public void LoadScene(string name)
        {
            StartCoroutine(LoadAsynchronously(name));
        }

        private void Start()
        {
            loadScene = () => LoadScene("Game");

            inputHandles.StartGameClick += loadScene;     
        }

        private void OnDisable()
        {
            inputHandles.StartGameClick -= loadScene;
        }

        private IEnumerator LoadAsynchronously(string Scene)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(Scene);

            loadingPanel.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);

                loadIndicator.fillAmount = progress;

                yield return null;
            }
        }
    }
}