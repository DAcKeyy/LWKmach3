using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
using System;

public class UpTimer : MonoBehaviour
{
    public static Action GameIsOver = delegate { };
    [SerializeField] private Counter TimeCounter;
    [BoxGroup("Time settings")]
    [Slider(5, 120)]
    public float roundTime = 30f;
    [BoxGroup("Time settings")]
    [Slider(5, 120)]
    public float timeToAdd = 10f;
    public Slider timeSlider;
    [BoxGroup("ThenTheTimeEnds")]
    [SerializeField] GameObject EndCanvas = null;

    private bool canProcess = true;
    private float initialTime;

    private void Start()
    {
        TimeCounter.SetText(Prefs.TimeBuff);
        timeSlider.minValue = 0f;
        timeSlider.maxValue = roundTime;
        initialTime = roundTime;
    }

    public void AddTime()
    {
        if(Prefs.TimeBuff != 0)
        {
            TimeCounter.SetText(--Prefs.TimeBuff);

            if (roundTime + timeToAdd > timeSlider.maxValue)
            {
                roundTime = initialTime;
            }
            else
            {
                roundTime += timeToAdd;
                timeSlider.value = roundTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canProcess)
        {
            roundTime -= Time.deltaTime;
            timeSlider.value = roundTime;

            if (roundTime <= 0f)
            {
                canProcess = false;
                roundTime = 0f;
                Debug.Log("Time is over");
                EndCanvas.SetActive(true);
                GameIsOver();
            }
        }
    }
}
