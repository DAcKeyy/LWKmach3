using System;
using UnityEngine;

public class SoundValueListener : MonoBehaviour
{
    private void OnEnable()
    {
        SetSoundValue.SoundValueChanged += SetValue;
    }

    private void OnDisable()
    {
        SetSoundValue.SoundValueChanged -= SetValue;
    }

    private void SetValue(float Value)
    {
        this.GetComponent<AudioSource>().volume = Value;
    }
}
