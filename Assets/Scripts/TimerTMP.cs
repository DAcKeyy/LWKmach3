using UnityEngine;
using TMPro;
using System;

public class TimerTMP : MonoBehaviour
{
    [SerializeField] TMP_Text TMP_text;
    public static Action TimeOver;

    public int days;
    public int hours;
    public int mins;
    public int seconds;


    private void Start()
    {
        InvokeRepeating("TimeDown", 1f, 1f);
    }


    void TimeDown()
    {
        seconds--;

        if(seconds == -1)
        {
            if (days == 0 && hours == 0 && mins == 0) TimeOver();
            else
            {
                mins--;
                seconds = 59;
            }

            if (mins == -1)
            {
                hours--;
                mins = 59;

                if (hours == -1)
                {
                    days--;
                    hours = 23;
                }
            }
        }

        if(days == 0)
        {
            if(hours == 0)
            {
                if(mins == 0)
                {
                    SetTimeOnText(seconds.ToString("0"));
                    return;
                }

                SetTimeOnText(mins.ToString("00") + ":" + seconds.ToString("00"));
                return;
            }

            SetTimeOnText(hours.ToString("00") + ":" + mins.ToString("00") + ":" + seconds.ToString("00"));
            return;          
        }

        SetTimeOnText(days.ToString("00") + ":" + hours.ToString("00") + ":" + mins.ToString("00") + ":" + seconds.ToString("00"));
    }

    void SetTimeOnText(string time)
    {
        TMP_text.text = time;
    }
}
