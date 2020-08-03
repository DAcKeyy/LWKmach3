using DG.Tweening;
using NaughtyAttributes;
using Spine.Unity;
using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;

namespace LWT.View//
{
    public class ChestView : MonoBehaviour
    {
        public static Action AnimationEnded;
        public static Action BombGotten;
        public static Action TimeGotten;
        //public static Action LootGotten;

        [BoxGroup("Prefabs")] [SerializeField] private GameObject chestPrefab = null;
        [BoxGroup("Prefabs")] [SerializeField] private GameObject lootPrefab = null;
        [BoxGroup("Images")] [SerializeField] private Sprite Bomb = null;
        [BoxGroup("Images")] [SerializeField] private Sprite Time = null;
        [BoxGroup("Default value")] [SerializeField] private AnimationReferenceAsset openAnimation = null;
        [BoxGroup("Default value")] [SerializeField] private float AnimationScale = 1;
        [BoxGroup("Tweens Duration")] [SerializeField] private float moveDuration = .2f;
        [BoxGroup("Tweens Duration")] [SerializeField] private float fadeDuration = .2f;
        [BoxGroup("Tweens Duration")] [SerializeField] private float scaleDuration = .2f;
        [BoxGroup("Tweens Duration")] [SerializeField] private float shakeDuration = .5f;
        [BoxGroup("Shake Tween")] [SerializeField] private Vector3 strength = new Vector3(0f, 100f, 0f);
        [BoxGroup("Transform Points")] [SerializeField] private Transform Center = null;
        [BoxGroup("Transform Points")] [SerializeField] private Transform BombEndPoint = null;
        [BoxGroup("Transform Points")] [SerializeField] private Transform TimeEndPoint = null;

        private GameObject chest;
        private GameObject loot;
        private ChestDrop chestDrop;
        private string DropParam1;
        private string DropParam2;
        private bool dropShown;

        private SkeletonAnimation skeletonAnimation;
        private Vector3 StartScale;

        private void OnEnable()
        {
            ChestManager.ChestOpening += GetChestDrop;
            AnimationEnded += DestroyAll;
        }

        private void OnDisable()
        {
            ChestManager.ChestOpening -= GetChestDrop;
            AnimationEnded -= DestroyAll;
        }

        void GetChestDrop(ChestDrop drop, string param1, string param2)
        {
            chestDrop = drop;
            DropParam1 = param1;
            DropParam2 = param2;
            OpenChest();
        }

        #region Animation
        public void OpenChest()
        {
            var chest = ChestInstantiate();


            chest.transform.DOScale(StartScale, scaleDuration).OnComplete(() =>
            chest.transform.DOShakePosition(shakeDuration, strength).OnComplete(() =>
            StartCoroutine(OpenChestNumerator())));
        }
        IEnumerator OpenChestNumerator()
        {
            chest.GetComponent<SkeletonAnimation>().state.SetAnimation(2, openAnimation, false).TimeScale = AnimationScale;

            yield return new WaitForSeconds(AnimationScale);

            ShowLoot();
        }

        public void ShowLoot()
        {
            loot = LootInstantiate();

            loot.transform.DOScale(1, scaleDuration).OnComplete(() => dropShown = true);
        }

        public void HideLoot()
        {
            if (dropShown)
            {
                HideChest();
                dropShown = false;
                switch (chestDrop)
                {
                    case ChestDrop.Coupon:
                        loot.GetComponentInChildren<SpriteRenderer>().DOFade(0, fadeDuration).SetEase(Ease.Linear).OnComplete(() => AnimationEnded());
                        break;
                    case ChestDrop.buff:
                        switch (DropParam1)
                        {
                            case "Bomb":
                                loot.transform.DOScale(0, moveDuration).OnComplete(()=> BombGotten());
                                loot.transform.DOMove(BombEndPoint.position, moveDuration).OnComplete(() => AnimationEnded());
                                break;
                            case "Overtime":
                                loot.transform.DOScale(0, moveDuration).OnComplete(() => TimeGotten()); ;
                                loot.transform.DOMove(TimeEndPoint.position, moveDuration).OnComplete(() => AnimationEnded());
                                break;
                            default:
                                Vector3 posiotion = new Vector3((BombEndPoint.position.x + TimeEndPoint.position.y) / 2,
                                    (BombEndPoint.position.y + TimeEndPoint.position.y) / 2,
                                    (BombEndPoint.position.z + TimeEndPoint.position.z) / 2);
                                loot.transform.DOMove(posiotion, moveDuration).OnComplete(() => AnimationEnded());
                                break;
                        }
                        break;
                }
            }
        }

        public void HideChest()
        {
            chest.transform.DOScale(0, scaleDuration/6).SetEase(Ease.Linear)/*.OnComplete(() => AnimationEnded())*/;
        }

        private void DestroyAll()
        {
            Destroy(loot);
            Destroy(chest);
        }


        #endregion

        private GameObject LootInstantiate()
        {
            loot = Instantiate(lootPrefab, Center);

            loot.SetActive(true);
            setText(loot);
            loot.transform.localScale = new Vector3(0f, 0f, 0f);

            return loot;
        }

        private void setText(GameObject loot)
        {
            switch (chestDrop)
            {
                case ChestDrop.Coupon:
                    loot.GetComponentInChildren<TMP_Text>().text = DropParam1 + "\n" + DropParam2;
                    break;
                case ChestDrop.buff:
                    switch (DropParam1)
                    {
                        case "Bomb":
                            loot.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Bomb;
                            break;
                        case "Overtime":
                            loot.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Time;
                            break;
                    }
                    break;          
            }
        }

        private GameObject ChestInstantiate()
        {
            chest = Instantiate(chestPrefab, Center);

            chest.SetActive(true);
            StartScale = chest.transform.localScale;
            chest.transform.localScale = new Vector3(0f, 0f, 0f);
            skeletonAnimation = chest.GetComponent<SkeletonAnimation>();

            //chest.transform.localScale = new Vector3(0f, 0f, 0f);            
            return chest;
        }
    }
}