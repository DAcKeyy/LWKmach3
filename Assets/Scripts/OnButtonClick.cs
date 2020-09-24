using UnityEngine;
using LWT.System;

public class OnButtonClick : MonoBehaviour
{
    [SerializeField] private string SceneName = null;
    [SerializeField] private bool openLeavesOnStart = true;

    public void Start()
    {
        if (openLeavesOnStart)
            FindObjectOfType<LevelLoader>().OpenLeaves();
    }

    public void Click()
    {
        FindObjectOfType<LevelLoader>().LoadScene(SceneName);
    }
}
