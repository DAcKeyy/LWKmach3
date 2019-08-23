using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;
using System;

public class UpTimer : MonoBehaviour
{
    [BoxGroup("Time settings")]
    [Slider(5, 120)]
    public float roundTime = 30f;
    [BoxGroup("Time settings")]
    [Slider(5, 120)]
    public float timeToAdd = 10f;
    public Slider timeSlider;
    [BoxGroup("ThenTheTimeEnds")]
    [SerializeField] GameObject EndPanel = null;

    [BoxGroup("Score")]
    [SerializeField] TMP_Text ScoreText = null;
    [BoxGroup("Level")]
    [SerializeField] TMP_Text LevelText = null;

    private bool canProcess = true;
    private float initialTime;

    private void OnEnable()
    {
        BoardManager.OnScoreCounted += ScoreCount;
        Board.BoardUp += AddLvl;
    }

    private void OnDisable()
    {
        BoardManager.OnScoreCounted += ScoreCount;
        Board.BoardUp += AddLvl;
    }

    private void Start()
    {
        timeSlider.minValue = 0f;
        timeSlider.maxValue = roundTime;
        initialTime = roundTime;
        AddLvl();
    }

    public void AddTime()
    {
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
                EndPanel.SetActive(true);
            }
        }
    }

    private void ScoreCount(int picesDeletedCount)
    {
        var scoreText = Convert.ToInt32(ScoreText.text);

        scoreText += picesDeletedCount * 100; //Шаманить тут 

        ScoreText.text = Convert.ToString(scoreText);
    }

    private void AddLvl()
    {
        var lvlText = Convert.ToInt32(LevelText.text);

        lvlText += 1;

        LevelText.text = Convert.ToString(lvlText);
    }
}
