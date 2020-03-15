using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;
using LWT.System;

namespace LWT.UI.Menu
{
    public class SettingMenu : MonoBehaviour
    {
        [Inject]
        private InputHandles inputHandles;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Show(float time)
        {
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Hide(float time)
        {
        }

        private void Start()
        {
            inputHandles.BackToMenu +=Hide;
            inputHandles.SettingClick += Show;
        }
    }
}