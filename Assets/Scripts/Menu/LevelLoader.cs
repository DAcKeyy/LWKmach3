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
            Debug.Log("Awake" + SceneManager.GetActiveScene().name);
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
            if (AnimationComponent != null)
            {
                if (open)
                {
                    AnimationComponent.Play(LeavesOpen.name);

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