using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole Instance;

    [SerializeField] private Transform logContainer;
    [SerializeField] private GameObject logLinePrefab;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.logMessageReceived += HandleLog;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        GameObject logLine = Instantiate(logLinePrefab, logContainer);
        TextMeshProUGUI text = logLine.GetComponent<TextMeshProUGUI>();

        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                text.color = Color.red;
                break;
            case LogType.Warning:
                text.color = Color.yellow;
                break;
            default:
                text.color = Color.white;
                break;
        }

        text.text = logString;

        // Update layout and scroll to bottom
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(logContainer.GetComponent<RectTransform>());
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
