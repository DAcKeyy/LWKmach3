using UnityEngine;
using TMPro;

public class VesionTextUpdate : MonoBehaviour
{
    void OnEnable()
    {
        this.GetComponent<TMP_Text>().text = ("ver " + Application.version);        
    }
}
