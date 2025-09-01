using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    [Header("Refs")]
    public GridManager gridManager;     // O anki leveli buradan alacağız (Inspector’dan atayın)
    public Button menuButton;           // UI Button
    [Header("Scene")]
    public string sceneName = "Demo1";  // Build Settings’e eklenmiş olmalı

    void Awake()
    {
        if (menuButton == null)
            menuButton = GetComponent<Button>();

        if (menuButton != null)
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(OnMenuButtonClicked);
        }
        else
        {
            Debug.LogError("[MenuButtonController] Button referansı yok!");
        }
    }

    private void OnMenuButtonClicked()
    {
        // 1) Level’i kaydet
        if (gridManager != null)
        {
            SaveSystem.SaveLevel(gridManager.currentLevelIndex);
        }
        else
        {
            Debug.LogWarning("[MenuButtonController] GridManager atanmadı, level kaydedilemedi!");
        }

        // 2) Demo1 sahnesine git
        Debug.Log($"[MenuButtonController] Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
