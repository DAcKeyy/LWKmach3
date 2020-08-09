using UnityEngine;
using TMPro;
using System;

public class ChestsCounter : MonoBehaviour
{
    [SerializeField] TMP_Text CountText = null;
    private int count;

    private void OnEnable()
    {
        ChestAnimationController.ChestCompleted += Increment;
    }

    private void OnDisable()
    {
        ChestAnimationController.ChestCompleted -= Increment;
    }

    void Increment()
    {
        count++;

        CountText.text = count.ToString();
    }
}
