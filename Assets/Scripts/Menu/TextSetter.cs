using UnityEngine;
using TMPro;

public class TextSetter : MonoBehaviour
{
    [SerializeField] string String = null;
    [SerializeField] string[] OverloadString = null;
    [SerializeField] bool ItIsFromObject = false;
    private TextMeshProUGUI Text;
    

    void Start()
    {
        Text = this.GetComponent<TextMeshProUGUI>();

        InvokeRepeating("TextSetter", 3,3);
    }

    void TextSetterMetod()
    {
        if (ItIsFromObject)
        {


            Text.SetText(String);
        }
        else
        {
            Text.SetText(String + OverloadString[0] + OverloadString[1] + OverloadString[2] + OverloadString[3]);
        }

    }
}
