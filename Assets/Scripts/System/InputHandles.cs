using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LWT.System
{
    /// <summary>
    /// Обработчик UI элементов
    /// </summary>
    public class InputHandles : IInitializable
    {
        public event Action StartGameClick;
        public event Action CouponClick;
        public event Action SettingClick;

        private readonly UIElements elements;

        public InputHandles(UIElements elements)
        {
            this.elements = elements;
        }

        public void Initialize()
        {
            elements.StartGame.onClick.AddListener(() => StartGameClick?.Invoke());
            elements.Coupon.onClick.AddListener(() => CouponClick?.Invoke());
            elements.Setting.onClick.AddListener(() => SettingClick?.Invoke());
        }

        [Serializable]
        public class UIElements
        {
            public Button StartGame;
            public Button Coupon;
            public Button Setting;
        }
    }
}