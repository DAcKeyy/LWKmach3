using UnityEngine;
using LWT.System;
using Zenject;

namespace LWT.UI.StartScene
{
    public class RegistrationPanel : MonoBehaviour
    {
        [Inject]
        private StartSceneInputHandels inputHandles = null;
        [SerializeField]
        private GameObject registrationPanel = null;

        public void Show()
        {
            registrationPanel.SetActive(true);
        }
        public void Hide()
        {
            registrationPanel.SetActive(false);
        }

        private void Start()
        {
            inputHandles.RegistrationClick += Show;
            inputHandles.RegistrationBackClick += Hide;
        }
    }
}
