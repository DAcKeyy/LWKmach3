using LWT.System;
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

        private void Start()
        {
            inputHandles.StartGameClick += () =>
            {               
                StartCoroutine(LoadAsynchronously("Game"));
            };
        }

        public void LoadScene(string name)
        {
            StartCoroutine(LoadAsynchronously(name));
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