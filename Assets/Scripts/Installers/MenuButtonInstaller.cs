using LWT.System;
using UnityEngine;
using Zenject;

namespace LWT.Installers
{
    public class MenuButtonInstaller : MonoInstaller
    {
        [SerializeField]
        private MenuInputHandels.MenuUIElements uIButtonElements = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuInputHandels>().AsSingle();
            Container.BindInstance(uIButtonElements);
        }
    }
}