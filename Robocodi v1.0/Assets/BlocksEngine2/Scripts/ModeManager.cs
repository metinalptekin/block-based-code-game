using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance { get; private set; }

    public enum Mode
    {
        Primary,
        Preschool
    }

    public Mode currentMode = Mode.Primary;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleMode()
    {
        string nextScene = currentMode == Mode.Primary ? "PreschoolModeScene" : "PrimaryModeScene";
        currentMode = currentMode == Mode.Primary ? Mode.Preschool : Mode.Primary;

        Debug.Log("Mod değiştiriliyor: " + nextScene);
        SceneManager.LoadScene(nextScene);
    }
}
