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
        public event Action BackToMenu;
        public event Action<float> MusicSliderChanged;
        public event Action<float> SoundSliderChanged;

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
            elements.CloseSettingMenu.onClick.AddListener(() => BackToMenu?.Invoke());
            elements.Music.onValueChanged.AddListener((value) => MusicSliderChanged?.Invoke(value));
            elements.Sound.onValueChanged.AddListener((value) => SoundSliderChanged?.Invoke(value));
        }

        [Serializable]
        public class UIElements
        {
            public Button StartGame;    
            public Button Coupon;
            public Button Setting;
            public Button CloseSettingMenu;
            public Slider Music;
            public Slider Sound;
        }
    }
}