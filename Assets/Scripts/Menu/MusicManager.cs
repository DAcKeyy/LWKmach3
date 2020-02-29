using UnityEngine;
using UnityEngine.UI;
using System;

public class MusicManager : MonoBehaviour
{
    public static Action<float> MusicValueChanged;
    readonly PlayerPreffsManager prefs = new PlayerPreffsManager();

    [SerializeField] Slider SliderObject = null;
    [SerializeField] AudioSource MusicSource = null;

    private void Start()
    {
        SliderObject.onValueChanged.AddListener(MusicValueChanged.Invoke);

        GlobalDataBase.MusicValue = prefs.GetMusicValue();
        MusicValueChanged(GlobalDataBase.MusicValue);
        SliderObject.value = GlobalDataBase.MusicValue;
    }

    private void OnEnable()
    {
        MusicManager.MusicValueChanged += SetValue;
    }

    private void OnDisable()
    {
        MusicManager.MusicValueChanged -= SetValue;
    }

    private void SetValue(float Value)
    {
        MusicSource.volume = Value;
    }

    public void FloatValue(Slider slider)
    {
        GlobalDataBase.MusicValue = slider.value;
        MusicValueChanged(GlobalDataBase.MusicValue);
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
