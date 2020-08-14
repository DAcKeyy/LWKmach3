﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Education : MonoBehaviour
{
    [SerializeField] GameObject educationCanvas;
    [SerializeField] List<GameObject> objectsToShow;
    [SerializeField] List<VerticalLayoutGroup> layoutGroups;
    [SerializeField] List<GameObject> objectsToDisable;
    [SerializeField] EducationText educationText;
    [SerializeField] List<TMP_Text> texts;
    [SerializeField] TMP_Text text;
    private string[] hints;
    private int Steps;
    private bool canTouch;

    private IEnumerator StartEducation()
    {
        yield return null;

        if (layoutGroups.Count > 0)
        {
            foreach (VerticalLayoutGroup grops in layoutGroups)
            {
                grops.enabled = false;
            }
        }

        hints = educationText.text.Split('|');
        Input.simulateMouseWithTouches = true;

        educationCanvas.SetActive(true);

        for (int i = 0; i < objectsToShow.Count; i++)
        {
            objectsToDisable[i].SetActive(false);
            objectsToShow[i].SetActive(false);
        }

        EducationStep();
    }

    void Start()
    {
        if (Prefs.IsFirstTime == 0) return;

        StartCoroutine(StartEducation());
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == 0)
                {
                    if (canTouch)
                    {
                        EducationStep();

                        canTouch = false;
                    }
                }
            }
        }

        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (canTouch)
                {
                    EducationStep();

                    canTouch = false;
                }
            }
        }
    }

    void EducationStep()
    {
        if (Steps >= objectsToShow.Count)
            EndEducation();
        else
        {
            DoScale(objectsToShow[Steps]);
            objectsToShow[Steps].SetActive(true);

            if(text != null) text.text = hints[Steps];
            else texts[Steps].text = hints[Steps];

            if (Steps > 0)
            {
                objectsToShow[Steps - 1].SetActive(false);
            }
            Steps++;
        }
    }

    void DoScale(GameObject image)
    {
        Debug.Log(Steps);
        Vector3 scale = image.transform.localScale;

        image.transform.DOScale(0.5f, 0f);        
        image.transform.DOScale(scale, 0.3f).OnComplete(() => OnComplete());
    }

    void OnComplete()
    {
        objectsToDisable[Steps - 1].SetActive(true);
        canTouch = true;
    }

    void EndEducation()
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            Prefs.IsFirstTime = 0;
        }

        educationCanvas.SetActive(false);   
    }
}
