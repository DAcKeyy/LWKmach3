using System;
using UnityEngine;

public class SoundValueListener : MonoBehaviour
{
    private void OnEnable()
    {
        SoundManager.SoundValueChanged += SetValue;
    }

    private void OnDisable()
    {
        SoundManager.SoundValueChanged -= SetValue;
    }

    private void SetValue(float Value)
    {
        this.GetComponent<AudioSource>().volume = Value;
    }
}
