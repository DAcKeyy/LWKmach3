using UnityEngine;
using LWT.System;
using Zenject;

namespace LWT.UI.StartScene
{
    public class AuthorizationPanel : MonoBehaviour
    {
        [Inject]
        private StartSceneInputHandels inputHandles = null;

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {            
            inputHandles.RegistrationBackClick += Show;
            inputHandles.RestorePasswordBackClick += Show;
            inputHandles.RegistrationClick += Hide;
            inputHandles.RestorePasswordClick += Hide;
        }
    }
}