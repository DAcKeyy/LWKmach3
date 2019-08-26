using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeelAnimationCode : MonoBehaviour
{
    [SerializeField] GameObject WinGiftPanel = null;
    
    private void OnEnable()
    {
        SpinCasinoWeel.EndRotate += PlayEndAnimation;
    }

    private void OnDisable()
    {
        SpinCasinoWeel.EndRotate -= PlayEndAnimation;
    }

    public void PlayStartAnimation(string Anim)
    {
        this.GetComponent<Button>().enabled = false;

        this.GetComponent<Animation>().Play(Anim);
    }

    public void OffPanel()
    {
        this.transform.Find("Panel").gameObject.SetActive(false);
    }

    private void PlayEndAnimation()
    {

        this.GetComponent<Animation>().Play("WeelDisappear");
    }

    private void ActivateWinGift()
    {
        string TestText = "Да";

        if (GloabalDataBase.NumberOfWeel == 1)
        {
            TestText = "хуй";
        }
        else if(GloabalDataBase.NumberOfWeel > 1 && GloabalDataBase.NumberOfWeel < 5)
        {
            TestText = "хуя";
        }
        else if (GloabalDataBase.NumberOfWeel >= 5)
        {
            TestText = "хуёв";
        }

        WinGiftPanel.SetActive(true);
        WinGiftPanel.transform.Find("Panel").transform.Find("Text (TMP)").
            gameObject.GetComponent<TMP_Text>().text =
            "Ты выйграл\n" + GloabalDataBase.NumberOfWeel + "\n" + TestText + " в жопу\n\n" + "Поздравляем!";

    }

    public void SetActive()
    {
        this.gameObject.SetActive(false);
    }
}
