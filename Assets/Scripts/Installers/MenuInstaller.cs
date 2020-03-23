using LWT.System;
using UnityEngine;
using Zenject;

namespace LWT.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField]
        private MenuInputHandels.MenuUIElements uIElements = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuInputHandels>().AsSingle();
            Container.BindInstance(uIElements);
        }
    }
}