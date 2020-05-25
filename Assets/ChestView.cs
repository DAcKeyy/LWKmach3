using DG.Tweening;
using NaughtyAttributes;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWT.View
{
    public class ChestView : MonoBehaviour
    {
        [BoxGroup("Default value")]
        [SerializeField]
        private GameObject chestPrefabs;

        [BoxGroup("Default value")]
        [SerializeField]
        private Vector3 defaultPosition = Vector3.zero;

        [BoxGroup("Scale Tween")]
        [SerializeField]
        private Vector2 defaultScale = new Vector2(140, 140);

        [BoxGroup("Default value")]
        [SerializeField]
        private AnimationReferenceAsset openAnimation;

        [SerializeField]
        [BoxGroup("Scale Tween")]
        private float duration = .2f;

        [SerializeField]
        [BoxGroup("Shake Tween")]
        private float shakeDuration = .5f;

        [SerializeField]
        [BoxGroup("Shake Tween")]
        private Vector3 strength = new Vector3(0f, 100f, 0f);

        [SerializeField]
        private Transform parent;

        private GameObject chest;

        public void OpenChest()
        {
            var chest = ChestGeneration();

            chest.transform
                .DOScale(defaultScale, duration)
                .SetEase(Ease.OutSine)
                .OnComplete(() => chest.transform.DOShakePosition(shakeDuration, strength)
                .OnComplete(() => chest.GetComponent<SkeletonAnimation>().state.SetAnimation(0, openAnimation, false).TimeScale = 1f)
           ); 
        }

        public void CloseChest()
        {
            chest.SetActive(false);
        }

        private GameObject ChestGeneration()
        {
            if (chest == null)
            {
                chest = Instantiate(chestPrefabs, parent);
            }

            chest.SetActive(true);
            chest.transform.localPosition = defaultPosition;
            chest.transform.localScale = new Vector3(0f, 0f, 0f);
            chest.GetComponent<SkeletonAnimation>().state.SetAnimation(0, openAnimation, false).TimeScale = 0f;
            return chest;
        }
    }
}