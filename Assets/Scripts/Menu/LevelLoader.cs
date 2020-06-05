using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649

namespace LWT.System
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private Animation AnimationComponent;
        [SerializeField]
        private AnimationClip LeavesClose;
        [SerializeField]
        private AnimationClip LeavesOpen;

        public void LoadScene(string name)
        {
            GlobalDataBase.PrevScene = SceneManager.GetActiveScene().name;

            StartCoroutine(LoadAsynchronously(name));
        }

        private IEnumerator LoadAsynchronously(string Scene)
        {
            yield return StartCoroutine(LeavesAnimation());

            AsyncOperation operation = SceneManager.LoadSceneAsync(Scene);

            while (!operation.isDone)
            {
                yield return null;
            }
        }

        private IEnumerator LeavesAnimation()
        {
            if (AnimationComponent != null)
            {
                AnimationComponent.gameObject.SetActive(true);

                AnimationComponent.Play(LeavesClose.name);

                while (AnimationComponent.IsPlaying(LeavesClose.name))
                {
                    yield return null;
                }
            }
        }
    }
}