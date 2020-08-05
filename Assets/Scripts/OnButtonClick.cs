using UnityEngine;
using LWT.System;

public class OnButtonClick : MonoBehaviour
{
    [SerializeField] private string SceneName;
    public void Start()
    {
        FindObjectOfType<LevelLoader>().OpenLeaves();
    }

    public void Click()
    {
        FindObjectOfType<LevelLoader>().LoadScene(SceneName);
    }
}
