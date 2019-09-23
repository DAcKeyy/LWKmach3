using System;
using UnityEngine;

public class SetSoundValue : MonoBehaviour
{
    public static Action<float> SoundValueChanged;

    public void Set(float Value)
    {
        GloabalDataBase.SoundValue = Value;
        SoundValueChanged(Value);
    }
}
