using UnityEngine;
using TMPro;
using System;

public class TimerTMP : MonoBehaviour
{
    [SerializeField] TMP_Text TMP_text;
    public static Action TimeOver;

    private int days;
    private int hours;
    private int mins;
    private int seconds;


    private void Start()
    {
        
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
                
                

                if(hours == -1)
                {
                    hours = 23;
                    if(days != 0)days--;
                   // else
                }
            }
        }

        if(days != 0)
        {
            SetTimeOnText(days + ":" + hours + ":" + mins + ":" + seconds);
        }

        SetTimeOnText(hours + ":" + mins + ":" + seconds);
    }

    void SetTimeOnText(string time)
    {
        TMP_text.text = time;
    }
}
