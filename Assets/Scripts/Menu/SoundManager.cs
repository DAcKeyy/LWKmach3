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
        GlobalDataBase.SoundValue = prefs.GetSoundValue();
        SoundValueChanged(GlobalDataBase.SoundValue);
        SliderObject.value = GlobalDataBase.SoundValue;
    }

    public void Set(Slider slider)
    {
        GlobalDataBase.SoundValue = slider.value;
        SoundValueChanged(GlobalDataBase.SoundValue);
    }

    #region Saving value to prefs
    private void OnApplicationPause(bool pause)
    {
        if(pause) prefs.SetSoundValue(GlobalDataBase.SoundValue);
    }

    private void OnApplicationQuit()
    {
        prefs.SetSoundValue(GlobalDataBase.SoundValue);
    }
    #endregion
}
