using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SpinCasinoWeel : MonoBehaviour
{
    [SerializeField] float MinWayValue = 0;
    [SerializeField] float MaxWayValue = 0;
    [SerializeField] float Speed = 0;

    private float TargetWay;
    private float Angle;
    private float zVelosirty;
    private float CoplexAngle;
    private int ActialRotation;
    private int CountTimes;

    Transform TragetRotation;

    WeelSection weelSection = new WeelSection();

    public static Action EndRotate = delegate { };

    public void Start()
    {
        WeelSection weelSection = new WeelSection();

        this.GetComponent<RectTransform>().transform.eulerAngles += new Vector3(0, 0, UnityEngine.Random.Range(0f,360f));
    }

    public void Spin()
    {
        this.GetComponent<Button>().enabled = false;

        TargetWay = UnityEngine.Random.Range(MinWayValue, MaxWayValue);

        Angle = ((TargetWay * 180) / (250 * Mathf.PI));
        CoplexAngle = (Angle / 360) + (transform.rotation.eulerAngles.z / 360);
        int IntCoplexAngle = (int)CoplexAngle;

        GloabalDataBase.NumberOfWeel = weelSection.WeelSectionConst((CoplexAngle - IntCoplexAngle));

       
        StopCoroutine("SpinWeel");
        StartCoroutine("SpinWeel");
    }

    private IEnumerator SpinWeel()
    {
        float Surent = 0;
        float Last = 0;
        float DIference = 0;

        while (true)
        {
            yield return null;

            Surent = Mathf.SmoothDamp(Surent, Angle, ref zVelosirty, Speed);
            DIference = Surent - Last;
            Last = Surent;

            this.GetComponent<RectTransform>().transform.eulerAngles += new Vector3(0, 0, Mathf.Abs(DIference));

            if (DIference == 0)
                break;
        }

        EndRotate();
    }
}

public class WeelSection
{
    public int WeelSectionConst(float Section)
    {
        int IntSection = 0;

        if(Section <= 0.125)
        {
            IntSection = 1;
        }
        else if(Section <= 0.25)
        {
            IntSection = 2;
        }
        else if (Section <= 0.375)
        {
            IntSection = 3;
        }
        else if (Section <= 0.5)
        {
            IntSection = 4;
        }
        else if (Section <= 0.625)
        {
            IntSection = 5;
        }
        else if (Section <= 0.75)
        {
            IntSection = 6;
        }
        else if (Section <= 0.875)
        {
            IntSection = 7;
        }
        else if (Section <= 1)
        {
            IntSection = 8;
        }

        return IntSection;
    }
}

