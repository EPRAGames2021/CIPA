using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
    static string myLog = "";
    private string lastLog = "";
    private string output;
    private string stack;
    private LogType _logType;

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        _logType = type;

        if (output == lastLog) return;
        if (type == LogType.Log) return;
        if (type == LogType.Warning) return;

        lastLog = output;

        myLog = output + "\n" + myLog;

        if (myLog.Length > 5000)
        {
            myLog = myLog[..4000];
        }
    }

    void OnGUI()
    {
        GUIStyle style = new(GUI.skin.label);

        Rect upperRect = new(90, 30, Screen.width * 0.9f, Screen.height * 0.6f);

        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = Screen.height * 3 / 100;

        switch (_logType)
        {
            case LogType.Error:
            case LogType.Exception:
            case LogType.Assert:
                style.normal.textColor = Color.red;
                break;
            case LogType.Warning:
                style.normal.textColor = Color.red;
                break;
            case LogType.Log:
                style.normal.textColor = Color.white;
                break;
        }

        GUI.Label(upperRect, myLog, style);
    }
}