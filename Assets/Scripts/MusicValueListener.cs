using UnityEngine;
using System;

public class MusicValueListener : MonoBehaviour
{
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
        this.GetComponent<AudioSource>().volume = Value;
    }
}
