using UnityEngine;
using TMPro;

public class CoinsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text TextToCoin = null;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        TextToCoin.text = "Coins: " + GlobalDataBase.Gold.ToString();
    }
}
