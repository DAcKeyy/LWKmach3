using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TweenTransform : MonoBehaviour
{
    //private Transform ObjectTransform;
    //public Action OnTweenScaleComplete;
    bool isDisbled;

    [SerializeField]
    private float Scale = 10f;
    [SerializeField]
    private float Delay = 5f;
    [SerializeField]
    private float Time = 0.4f;
    [SerializeField]
    private Ease ScaleEase = Ease.Unset;

    private void OnEnable()
    {
        StartAnimation();
    }

    private void OnDisable()
    {
        DOTween.Kill(this);

        StopCoroutine(Animation());
        StopCoroutine("DoScale");
    }

    private void StartAnimation()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return StartCoroutine(DoScale(0.1F, Delay, ScaleEase, null));
        yield return StartCoroutine(DoScale(Scale, Time, ScaleEase, null));
        yield return StartCoroutine(DoScale(-Scale, Time, ScaleEase, null));
        yield return StartCoroutine(DoScale(-0.1F, Delay, ScaleEase, StartAnimation));
        yield break;
    }

    private IEnumerator DoScale(float value, float time, Ease ease, Action EndScale)
    {
        transform.DOScale(new Vector3(transform.localScale.x + value,
                                    transform.localScale.y + value,
                                    transform.localScale.z), time)
                                    .SetEase(ease).OnComplete(() => EndScale?.Invoke());

        yield return null;
    }
} 