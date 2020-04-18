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
        isDisbled = false;
        AnimationONE();
    }

    private void OnDisable()
    {
        isDisbled = true;
        Debug.Log("ffdsa");
    }

    //!не смог в зынжект
    private void AnimationONE()
    {
        StartCoroutine(DoScale(0.1F, Delay, ScaleEase, AnimationTWO));
    }
    private void AnimationTWO()
    {
        StartCoroutine(DoScale(Scale, Time, ScaleEase, AnimationTHREE));
    }
    private void AnimationTHREE()
    {
        StartCoroutine(DoScale(-Scale, Time, ScaleEase, AnimationFORE));
    }
    private void AnimationFORE()
    {
        StartCoroutine(DoScale(-0.1F, Delay, ScaleEase, AnimationONE));
    }
    //

    private IEnumerator DoScale(float value, float time, Ease ease, Action EndScale)
    {
        transform.DOScale(new Vector3(transform.localScale.x + value,
                                    transform.localScale.y + value,
                                    transform.localScale.z), time)
                                    .SetEase(ease).OnComplete(() => EndScale());

        if (isDisbled == true)
        {
            DOTween.Kill(this);
            yield break;
        }

        else yield return null;//я хз как ошибку убрать
    }
} 