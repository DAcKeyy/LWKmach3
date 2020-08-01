using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] TMP_Text CountText = null;
    private int count;

    public void Increment()
    {
        count++;

        CountText.text = count.ToString();
    }
    public void SetText(string value)
    {
        CountText.text = value.ToString();
    }
}

