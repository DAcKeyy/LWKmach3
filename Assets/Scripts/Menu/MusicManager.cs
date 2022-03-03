using UnityEngine;
using UnityEngine.UI;
using System;

namespace LWT.System.Music
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private Toggle _musicToggle;
        
        private readonly PlayerPreffsManager _prefs = new PlayerPreffsManager();

        private void Start()
        {
            Debug.Log("sddas");
            SetMusicVolume(_prefs.GetMusicValue());
            GlobalDataBase.MusicValue = _prefs.GetMusicValue();

            if (_musicToggle == null) return;
            _musicToggle.isOn = (int)Math.Round(GlobalDataBase.MusicValue) == 1;
            _musicToggle.onValueChanged.AddListener((toggleValue) => {
                _prefs.SetMusicValue(toggleValue ? 1 : 0);
                SetMusicVolume(toggleValue ? 1 : 0);
            });
        }

        private void SetMusicVolume(float volume)
        {
            _musicSource.volume = volume;
            GlobalDataBase.MusicValue = volume;
        }

        #region Saving value to prefs
        private void OnApplicationPause(bool pause)
        {
            if (pause) _prefs.SetMusicValue(GlobalDataBase.MusicValue);
        }

        private void OnApplicationQuit()
        {
            _prefs.SetMusicValue(GlobalDataBase.MusicValue);
        }
        #endregion
    }
}