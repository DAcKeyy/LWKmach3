using UnityEngine;
using TMPro;

public class CoinsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text TextToCoin = null;

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        TextToCoin.text = "Coins: " + GlobalDataBase.Gold.ToString();
    }
}
