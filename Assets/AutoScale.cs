using UnityEngine;

public class AutoScale : MonoBehaviour
{
    private void Start()
    {
        RectTransform rt = this.GetComponent<RectTransform>();

        Debug.Log(Screen.height + " " + Screen.width);

        if(Screen.width > Screen.height)
        {
            rt.sizeDelta = new Vector2(Screen.height, Screen.height);
        }
        else
        {
            rt.sizeDelta = new Vector2(Screen.width, Screen.width);
        }
    }
}
