using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LWT.Installers
{
    [CreateAssetMenu(fileName = "Musics", menuName = "Libriary/MusicLibriary")]
    public class MusicLibriary : ScriptableObject
    {
        [SerializeField]
        private List<MusicItem> musicItems = new List<MusicItem>();

        [SerializeField]
        private List<MusicItem> soundItems = new List<MusicItem>();

        public AudioClip GetMusic(string name)
        {
            foreach (var music in musicItems)
            {
                if (music.Name == name)
                    return music.Clip;
            }

            Debug.LogError("Такого трека нет");
            return null;
        }

        public AudioClip GetSound(string name)
        {
            foreach (var sound in soundItems)
            {
                if (sound.Name == name)
                    return sound.Clip;
            }

            Debug.LogError("Такого звука нет");
            return null;
        }

        [Serializable]
        public class MusicItem
        {
            public AudioClip Clip;
            public string Name;
        }

        [Serializable]
        public class SoundItem
        {
            public AudioClip Clip;
            public string Name;
        }
    }
}