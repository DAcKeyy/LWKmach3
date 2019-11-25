using UnityEngine;
using TMPro;

public class VesionTextApdate : MonoBehaviour
{
    void OnEnable()
    {
        this.GetComponent<TMP_Text>().text = ("ver " + Application.version);        
    }
}
