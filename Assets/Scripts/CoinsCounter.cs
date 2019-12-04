using UnityEngine;
using TMPro;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField] TMP_Text text = null;

    private void OnEnable()
    {
        TaskManager.TaskComplete += SetText;
    }

    private void OnDisable()
    {
        TaskManager.TaskComplete -= SetText;
    }

    void SetText()
    {

        text.text = GlobalDataBase.Gold.ToString(); 
    }
}
