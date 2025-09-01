using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class WinPanelController : MonoBehaviour
{
    public GridManager gridManager;
    public Button playButton;
    public Button restartButton;
    public TMP_Text levelText;
    public Button menuButton;

    private bool listenersRegistered = false;

    void Awake()
    {
        RegisterListenersOnce();
    }

    void OnEnable()
    {
        UpdateLevelText();
    }

    private void RegisterListenersOnce()
    {
        if (listenersRegistered) return;

        if (playButton != null)
        {
            // Olası önceki eklemeleri temizle
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        if (menuButton != null)
        {
            menuButton.onClick.RemoveListener(OnMenuButtonClicked);
            menuButton.onClick.AddListener(OnMenuButtonClicked);
        }


        listenersRegistered = true;
        Debug.Log("[WinPanelController] Listeners registered once.");
    }

    void OnPlayButtonClicked()
    {
        if (gridManager == null)
        {
            Debug.LogWarning("WinPanelController içinde GridManager atanmadı!");
            return;
        }

        int before = gridManager.currentLevelIndex + 1;
        Debug.Log($"[WinPanelController] Play clicked. Current Level (before NextLevel): {before}");

        gridManager.NextLevel();

        int after = gridManager.currentLevelIndex + 1;
        Debug.Log($"[WinPanelController] After NextLevel -> Current Level: {after}");

        if (gridManager.player != null)
            gridManager.player.ResetToStartPosition();

        if (gridManager.winPanel != null)
            gridManager.winPanel.SetActive(false);

        UpdateLevelText();
    }
    

    void OnRestartButtonClicked()
    {
        if (gridManager == null)
        {
            Debug.LogWarning("WinPanelController içinde GridManager atanmadı!");
            return;
        }

        Debug.Log($"[WinPanelController] Restart clicked. Restarting Level {gridManager.currentLevelIndex + 1}");

        gridManager.RestartLevel();

        if (gridManager.winPanel != null)
            gridManager.winPanel.SetActive(false);

        UpdateLevelText();
    }

    public void UpdateLevelText()
    {
        if (levelText != null && gridManager != null)
        {
            int displayLevel = gridManager.currentLevelIndex + 1;
            levelText.text = "Level " + displayLevel;
            Debug.Log($"[WinPanelController] Level text updated -> Level {displayLevel}");
        }
    }
    void OnMenuButtonClicked()
    {
            Debug.Log("[WinPanelController] Menu button clicked. Loading RobocodiGiris scene...");
            SceneManager.LoadScene("RobocodiGiris");
    }
}
