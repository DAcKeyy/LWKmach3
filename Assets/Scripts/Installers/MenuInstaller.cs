using LWT.System;
using UnityEngine;
using Zenject;

namespace LWT.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField]
        private InputHandles.UIElements uIElements;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputHandles>().AsSingle();
            Container.BindInstance(uIElements);
        }
    }
}