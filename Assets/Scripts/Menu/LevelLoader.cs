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

        private void Awake()
        {
            //Debug.Log("Awake" + SceneManager.GetActiveScene().name);
            //OpenLeaves();
            DontDestroyOnLoad(this);
        }

        public void LoadSceneOnly(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void LoadScene(string name)
        {
            StartCoroutine(LoadAsynchronously(name));
        }

        public void OpenLeaves()
        {
            Vector3 camPos = Camera.main.transform.position;
            transform.position = new Vector3(camPos.x - 0.8f, camPos.y, 1.7f);
            StartCoroutine(LeavesAnimation(true));
        }

        private IEnumerator LoadAsynchronously(string Scene)
        {
            yield return StartCoroutine(LeavesAnimation(false));

            AsyncOperation operation = SceneManager.LoadSceneAsync(Scene);

            while (!operation.isDone)
            {
                yield return null;
            }
        }

        private IEnumerator LeavesAnimation(bool open)
        {
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;

            if (AnimationComponent != null)
            {
                if (open)
                {
                    AnimationComponent.Play(LeavesOpen.name);
                    Debug.Log("Open");
                    while (AnimationComponent.IsPlaying(LeavesOpen.name))
                        yield return null;
                }
                else
                {
                    AnimationComponent.Play(LeavesClose.name);

                    while (AnimationComponent.IsPlaying(LeavesClose.name))
                        yield return null;
                }
            }
        }
    }
}