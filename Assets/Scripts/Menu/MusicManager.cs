using UnityEngine;
using UnityEngine.UI;
using System;

public class MusicManager : MonoBehaviour
{
    public static Action<float> MusicValueChanged;
    readonly PlayerPreffsManager prefs = new PlayerPreffsManager();

    [SerializeField] Slider SliderObject = null;

    private void Start()
    {
        GloabalDataBase.MusicValue = prefs.GetMusicValue();
        MusicValueChanged(GloabalDataBase.MusicValue);
        SliderObject.value = GloabalDataBase.MusicValue;
    }

    public void Set(Slider slider)
    {
        GloabalDataBase.MusicValue = slider.value;
        MusicValueChanged(GloabalDataBase.MusicValue);
    }

    #region Saving value to prefs
    private void OnApplicationPause(bool pause)
    {
        if (pause) prefs.SetMusicValue(GloabalDataBase.MusicValue);
    }

    private void OnApplicationQuit()
    {
        prefs.SetMusicValue(GloabalDataBase.MusicValue);
    }
    #endregion
}
