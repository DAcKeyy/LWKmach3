using LWT.System;
using UnityEngine;
using Zenject;

namespace LWT.Installers
{
    public class StartInstaller : MonoInstaller
    {
        [SerializeField]
        private StartSceneInputHandels.StartUIElements uIElements = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StartSceneInputHandels>().AsSingle();
            Container.BindInstance(uIElements);
        }
    }
}

