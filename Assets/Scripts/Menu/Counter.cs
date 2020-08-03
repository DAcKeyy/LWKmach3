using System;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] TMP_Text CountText = null;
    private int count;

    public void OnEnable()
    {
        if (CountText == null)
            CountText = this.GetComponent<TMP_Text>();
    }

    public void Increment()
    {
        count++;
        CountText.text = count.ToString();
    }
    public void Decrement()
    {
        count--;
        CountText.text = count.ToString();
    }

    public void SetText(string value)
    {
        CountText.text = value.ToString();
        count = Convert.ToInt32(CountText.text);
    }
    public void SetText(int value)
    {
        CountText.text = value.ToString();
        count = Convert.ToInt32(CountText.text);
    }
    public void SetText(float value)
    {
        CountText.text = value.ToString();
        count = Convert.ToInt32(CountText.text);
    }
}

