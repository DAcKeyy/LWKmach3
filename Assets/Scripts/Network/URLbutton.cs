using UnityEngine.EventSystems;
using UnityEngine;

public class URLbutton : MonoBehaviour, IPointerClickHandler
{
    public string URLOnButton;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Application.OpenURL(URLOnButton);
    }
}
