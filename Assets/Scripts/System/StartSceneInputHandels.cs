using System;
using UnityEngine.UI;
using Zenject;

namespace LWT.System
{
    public class StartSceneInputHandels : IInitializable
    {
        public event Action LoginClick;
        public event Action ShowRegistrationClick;
        public event Action RegistrationBackClick;
        public event Action ShowRestorePasswordClick;
        public event Action RestorePasswordBackClick;
        public event Action RegistrationClick;
        public event Action RestorePasswordClick;

        private readonly StartUIElements startElements;
        public StartSceneInputHandels(StartUIElements startElements)
        {
            this.startElements = startElements;
        }

        public void Initialize()
        {
            startElements.Login.onClick.AddListener(() => LoginClick?.Invoke());
            startElements.ShowRegistration.onClick.AddListener(() => ShowRegistrationClick?.Invoke());
            startElements.RegistrationBack.onClick.AddListener(() => RegistrationBackClick?.Invoke());
            startElements.ShowRestorePassword.onClick.AddListener(() => ShowRestorePasswordClick?.Invoke());
            startElements.RestorePasswordBack.onClick.AddListener(() => RestorePasswordBackClick?.Invoke());
            startElements.Registration.onClick.AddListener(() => RegistrationClick?.Invoke());
            startElements.RestorePassword.onClick.AddListener(() => RestorePasswordClick?.Invoke());
        }

        [Serializable]
        public class StartUIElements
        {
            public Button Login;
            public Button ShowRegistration;
            public Button ShowRestorePassword;
            public Button RegistrationBack;
            public Button RestorePasswordBack;
            public Button Registration;
            public Button RestorePassword;
        }
    }
}
