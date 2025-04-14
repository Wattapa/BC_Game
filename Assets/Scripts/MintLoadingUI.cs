using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MintLoadingUI : MonoBehaviour
{
    public static MintLoadingUI Instance;

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Show(string message = "Minting in progress...")
    {
        loadingPanel.SetActive(true);
        loadingText.text = message;
    }

    public void Hide()
    {
        loadingPanel.SetActive(false);
    }
}