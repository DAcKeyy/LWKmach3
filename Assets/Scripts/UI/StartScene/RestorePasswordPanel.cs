using UnityEngine;
using LWT.System;
using Zenject;

namespace LWT.UI.StartScene
{
    public class RestorePasswordPanel : MonoBehaviour
    {
        [Inject]
        private StartSceneInputHandels inputHandles = null;
        [SerializeField]
        private GameObject restorePasswordPanel = null;

        public void Show()
        {
            restorePasswordPanel.SetActive(true);
        }

        public void Hide()
        {
            restorePasswordPanel.SetActive(false);
        }

        private void Start()
        {
            inputHandles.ShowRestorePasswordClick += Show;
            inputHandles.RestorePasswordBackClick += Hide;
        }

        private void OnDisable()
        {
            inputHandles.ShowRestorePasswordClick -= Show;
            inputHandles.RestorePasswordBackClick -= Hide;
        }
    }
}
