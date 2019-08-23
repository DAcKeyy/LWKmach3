using UnityEngine;
using UnityEngine.UI;

public class WeelAnimationCode : MonoBehaviour
{
    [SerializeField] GameObject WinGiftPanel;
    
    private void OnEnable()
    {
        SpinCasinoWeel.EndRotate += PlayEndAnimation;
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
        WinGiftPanel.SetActive(true);
    }

    public void SetActive()
    {
        this.gameObject.SetActive(false);
    }
}
