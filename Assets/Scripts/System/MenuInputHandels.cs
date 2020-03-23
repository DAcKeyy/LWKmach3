using System;
using UnityEngine.UI;
using Zenject;

namespace LWT.System
{
    public class MenuInputHandels : IInitializable
    {
        public event Action StartGameClick;
        public event Action CouponClick;
        public event Action SettingClick;

        private readonly MenuUIElements menuElements;

        public MenuInputHandels(MenuUIElements menuElements)
        {
            this.menuElements = menuElements;
        }

        public void Initialize()
        {
            menuElements.StartGame.onClick.AddListener(() => StartGameClick?.Invoke());
            menuElements.Coupon.onClick.AddListener(() => CouponClick?.Invoke());
            menuElements.Setting.onClick.AddListener(() => SettingClick?.Invoke());
        }

        [Serializable]
        public class MenuUIElements
        {
            public Button StartGame;
            public Button Coupon;
            public Button Setting;
        }
    }
}
