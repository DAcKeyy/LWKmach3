using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeelAnimationCode : MonoBehaviour
{
    [SerializeField] GameObject WinGiftPanel = null;
    
    private void OnEnable()
    {
        RouletteMover.EndRotate += PlayEndAnimation;
    }

    private void OnDisable()
    {
        RouletteMover.EndRotate -= PlayEndAnimation;
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

        Debug.Log("Ended");

        this.GetComponent<Animation>().Play("WeelDisappear");
    }

    private void ActivateWinGift()
    {
        WinGiftPanel.SetActive(true);
        WinGiftPanel.transform.Find("Panel").transform.Find("Text (TMP)").
            gameObject.GetComponent<TMP_Text>().text =
            "Ты выйграл\n" + GloabalDataBase.NumberOfWeel + "\n" + "Поздравляем!";
    }

    public void SetActive()
    {
        this.gameObject.SetActive(false);
    }
}
