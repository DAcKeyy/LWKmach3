using UnityEngine;
using System.Collections;

public class GUI_Console : MonoBehaviour
{
    private bool consoleishidden;
    private string output;
    private string stack;
    public GUISkin consoleskin;
    private Vector2 scroll;

    void OnGUI() 
    {
        GUI.skin = consoleskin;
        GUI.depth = -10000;

        if (GUILayout.Button("Q"))
        {
            consoleishidden = !consoleishidden;
        }
        if (consoleishidden)
        {
            ShowConsole();
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
