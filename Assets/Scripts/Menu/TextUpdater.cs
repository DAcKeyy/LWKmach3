using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{



    private TextMeshProUGUI Text;

    private enum Value {
        UserName = 0,
        Gold = 1,
    };

    [SerializeField] Value value = 0;



    void FixedUpdate()
    {
        Text = this.GetComponent<TextMeshProUGUI>();    //this = текст основной волюты

        switch ((int)value)
        {
            case 0:                                                      
                Text.SetText(GlobalDataBase.NickName);          //Имя пользователя
                break;
            case 1:
                Text.SetText(GlobalDataBase.Gold.ToString());
                break;
        }
    }
}
