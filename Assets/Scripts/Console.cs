using UnityEngine;
using System.Collections;

public class Console : MonoBehaviour
{
    private bool consoleishidden;
    private string output;
    private string stack;
    public GUISkin consoleskin;
    private Vector2 scroll;

    void Start()
    {
        ShowHideConsole();
    }

    void OnGUI() 
    {
        GUI.skin = consoleskin;
        GUI.depth = -10000;
        if (consoleishidden)
        {
            ShowConsole();
        }
    }
    void ShowHideConsole()
    {
        if (consoleishidden)
        {
            consoleishidden = false;
        }
        else
        {
            consoleishidden = true;
        }
    }
    void ShowConsole()
    {
        GUILayout.BeginArea(new Rect(0, 5, Screen.width, Screen.height / 2));
        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.Label(output);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output += type + ": " + logString + "\n";
        stack += stackTrace;
        scroll.y = 10000000000;
    }
}
