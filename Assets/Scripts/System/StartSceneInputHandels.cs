using System;
using UnityEngine.UI;
using Zenject;

namespace LWT.System
{
    public class StartSceneInputHandels : IInitializable
    {
        public event Action LoginClick;
        public event Action RegistrationClick;
        public event Action RestorePasswordClick;
        public event Action ResendEmailClick;

        private readonly StartUIElements startElements;
        public StartSceneInputHandels(StartUIElements startElements)
        {
            this.startElements = startElements;
        }

        public void Initialize()
        {
            startElements.Login.onClick.AddListener(() => LoginClick?.Invoke());
            startElements.Registration.onClick.AddListener(() => RegistrationClick?.Invoke());
            startElements.RestorePassword.onClick.AddListener(() => RestorePasswordClick?.Invoke());
            startElements.ResendEmail.onClick.AddListener(() => ResendEmailClick?.Invoke());
        }

        [Serializable]
        public class StartUIElements
        {
            public Button Login;
            public Button Registration;
            public Button RestorePassword;
            public Button ResendEmail;
        }
    }
}
