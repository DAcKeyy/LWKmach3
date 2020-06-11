using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;

namespace LWT.System.Music
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource musicSource = null;

        //[Inject]
        //private InputHandles inputHandles = null;

        private readonly PlayerPreffsManager prefs = new PlayerPreffsManager();

        private void Start()
        { 
            GlobalDataBase.MusicValue = prefs.GetMusicValue();
            //inputHandles.MusicSliderChanged += SetMusicVolume;
        }

        private void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
            GlobalDataBase.MusicValue = volume;
        }

        #region Saving value to prefs
        private void OnApplicationPause(bool pause)
        {
            if (pause) prefs.SetMusicValue(GlobalDataBase.MusicValue);
        }

        private void OnApplicationQuit()
        {
            prefs.SetMusicValue(GlobalDataBase.MusicValue);
        }
        #endregion
    }
}