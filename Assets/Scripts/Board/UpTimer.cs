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
    [BoxGroup("Other Stuff")]
    [SerializeField] private AudioSource TimeUpAudioSource = null;
    [BoxGroup("Other Stuff")]
    [SerializeField] private AudioSource LowTimeAudioSource = null;
    [BoxGroup("Other Stuff")]
    [SerializeField] private AudioSource StartAudioSource = null;
    private bool canProcess = true;
    private float initialTime;

    public void StopTimer()
    {
        Debug.Log("ZAA WAARDO");
        canProcess = !canProcess;
    }

    private void OnEnable()
    {
        ChestAnimationController.TaskCompleted += TaskListener;
        ChestAnimationController.ChestCompleted += ChestListener;
        Education.EducationEnded += StopTimer;
    }

    private void OnDisable()
    {
        ChestAnimationController.TaskCompleted -= TaskListener;
        ChestAnimationController.ChestCompleted -= ChestListener;
        Education.EducationEnded -= StopTimer;
    }

    private void Start()
    {
        if (Prefs.IsFirstTime == 1) canProcess = false;

        Debug.Log(canProcess);

        StartAudioSource.Play();
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

            TimeUpAudioSource.Play();
        }
    }

    private void ChestListener()
    {
        AddTimeFlexible(1.5f);
        GlobalDataBase.ChestEarned++;
    }

    private void TaskListener()
    {
        AddTimeFlexible(0.833f);
    }

    private void AddTimeFlexible(float seconds)
    {
        if (roundTime + seconds > timeSlider.maxValue)
        {
            roundTime = initialTime;
        }
        else
        {
            roundTime += seconds;
            timeSlider.value = roundTime;
        }

    }

    private void FixedUpdate()
    {
        if (canProcess)
        {           
            roundTime -= Time.deltaTime;
            GlobalDataBase.RoundTime += Time.deltaTime;
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
