using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class ChestAnimationController : MonoBehaviour
{
    public static Action ChestCompleted;

    [SerializeField] private Animator ChestAnimator = null;
    [SerializeField] private Transform CounterPosition = null;
    [SerializeField] private float Speed = 1;
    [SerializeField] private GameObject Chest;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] AudioClip ChainOff = null;
    [SerializeField] AudioClip ChestAppear = null;
    [SerializeField] AudioClip ChestDisappear = null;

    private Vector3 StartPosition;
    private Vector3 StartScale;
    private int TaskCounter;

    private void OnEnable()
    {
        TaskManager.TaskComplete += Animation;
    }
    private void OnDisable()
    {
        TaskManager.TaskComplete -= Animation;
    }

    private void Start()
    {
        StartPosition = transform.position;
        StartScale = transform.localScale;

        MoveToStart();
    }

    void SpawnChest() //Просто перенос сундука вправо
    {  
        Vector3 spawnPosition = new Vector3(5f, CounterPosition.position.y,0);
        gameObject.transform.position = spawnPosition;

        ChestAnimator.Rebind();
        MoveToStart();
    }

    void MoveToStart()
    {
        audioSource.clip = ChestAppear;
        audioSource.panStereo = 1f;
        audioSource.Play();
        gameObject.transform.DOMove(StartPosition, Speed*2).SetEase(ease);
        gameObject.transform.DOScale(StartScale, Speed*2).SetEase(ease);
    }

    void Animation()//на каждое выполненое задание идёт своя анимация (см контролер сундука)
    {
        TaskCounter++;

        if (TaskCounter == 1)
        {
            audioSource.clip = ChainOff;
            audioSource.panStereo = 0f;
            audioSource.Play();
            ChestAnimator.SetTrigger("FirstTask");
        }
        if (TaskCounter == 2)
        {           
            audioSource.Play();
            ChestAnimator.SetTrigger("SecondTask");
        }
        if (TaskCounter == 3)//тут не придумал как это сделать в контролере поэтому сделал это с помощью твина
        {//на самом деле придумал (Адель), но в падлу переписывать 
            TaskCounter = 0;
            audioSource.clip = ChestDisappear;
            audioSource.panStereo = -1f;
            audioSource.Play();
            StartCoroutine(TweenAnimaton());
        }
    }

    IEnumerator TweenAnimaton()
    {
        gameObject.transform.DOScale(gameObject.transform.localScale.x + gameObject.transform.localScale.x * 0.05f, Speed).SetEase(ease);
        yield return new WaitForSeconds(Speed);
        gameObject.transform.DOScale(gameObject.transform.localScale.x - gameObject.transform.localScale.x * 0.05f, Speed).SetEase(ease);
        yield return new WaitForSeconds(Speed);
        gameObject.transform.DOMove(CounterPosition.position, Speed).SetEase(ease).OnComplete(() => ChestCompleted?.Invoke());
        gameObject.transform.DOScale(0, Speed).SetEase(ease).OnComplete(() => SpawnChest());

    }
}
