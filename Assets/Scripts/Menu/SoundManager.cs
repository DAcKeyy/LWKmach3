using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static Action<float> SoundValueChanged;
    PlayerPreffsManager prefs = new PlayerPreffsManager();

    [SerializeField] Slider SliderObject = null;

    private void Start()
    {        
        GloabalDataBase.SoundValue = prefs.GetSoundValue();
        SoundValueChanged(GloabalDataBase.SoundValue);
        SliderObject.value = GloabalDataBase.SoundValue;
    }

    public void Set(Slider slider)
    {
        GloabalDataBase.SoundValue = slider.value;
        SoundValueChanged(GloabalDataBase.SoundValue);
    }

    #region Saving value to prefs
    private void OnApplicationPause(bool pause)
    {
        if(pause) prefs.SetSoundValue(GloabalDataBase.SoundValue);
    }

    private void OnApplicationQuit()
    {
        prefs.SetSoundValue(GloabalDataBase.SoundValue);
    }
    #endregion
}
