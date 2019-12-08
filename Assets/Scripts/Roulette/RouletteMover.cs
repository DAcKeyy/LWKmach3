using DG.Tweening;
using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System;

public class RouletteMover : MonoBehaviour
{
    public static Action<int> EndRotate = delegate { };
    public static Action StartRotate = delegate { };

    [BoxGroup("Roulette")]
    public int sectorCount = 8;
    [BoxGroup("Roulette")]
    public float spinTime = 2;

    [Space(5)]

    [BoxGroup("Sector")]
    public int needSector = 5;

    private Roulette roulette;
    private Transform rouletteObject;

    public void Spin()
    {
        StartRotate();
        rouletteObject = this.transform;
        roulette = new Roulette(sectorCount);
        needSector = UnityEngine.Random.Range(1, sectorCount);
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        yield return null; //For unity lag on start
        rouletteObject.DORotate(new Vector3(0f, 0f, roulette.Spin(needSector, true)), spinTime, RotateMode.WorldAxisAdd).SetEase(Ease.InOutCubic).OnComplete(() => EndRotate(needSector));
    }
}