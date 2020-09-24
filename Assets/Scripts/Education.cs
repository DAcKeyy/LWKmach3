using DG.Tweening;
using ModestTree;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NaughtyAttributes;
using System;

public class Education : MonoBehaviour
{
    public static Action EducationEnded;
    [SerializeField] float ScaleTime = 0.3f;
    [SerializeField] GameObject educationCanvas;
    [BoxGroup("Objects To Show")] [SerializeField] List<GameObject> objectsToShow = null;
    [BoxGroup("Layouts")] [SerializeField] List<VerticalLayoutGroup> layoutGroups  = null;
    [BoxGroup("Objects To Disable")]  [SerializeField] List<GameObject> objectsToDisable = null;
    [SerializeField] EducationText educationText;
    [BoxGroup("Education Texts")] [SerializeField] List<TMP_Text> texts;
    [SerializeField] TMP_Text text;
    private string[] hints;
    private int Steps;
    private bool canTouch;

    private IEnumerator StartEducation()
    {
        yield return null;

        if(!layoutGroups.IsEmpty())
        {
            if (layoutGroups.Count > 0)
            {
                foreach (VerticalLayoutGroup grops in layoutGroups)
                {
                    grops.enabled = false;
                }
            }
        }

        hints = educationText.text.Split('|');
        Input.simulateMouseWithTouches = true;

        educationCanvas.SetActive(true);

        for (int i = 0; i < objectsToShow.Count; i++)
        {
            //objectsToDisable[i].SetActive(false);
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

        image.transform.DOScale(0.95f, ScaleTime);        
        image.transform.DOScale(scale, ScaleTime).OnComplete(() => OnComplete());
    }

    void OnComplete()
    {
        if(!objectsToDisable.IsEmpty())
            objectsToDisable[Steps - 1].SetActive(true);

        canTouch = true;
    }

    void EndEducation()
    {


        if(SceneManager.GetActiveScene().name == "Game")
        {
            Prefs.IsFirstTime = 0;
            EducationEnded();
        }

        educationCanvas.SetActive(false);   
    }
}
