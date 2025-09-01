using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    public Button buttonPrimary;
    public Button buttonPreschool;

    void Start()
    {
        // Buttonlara tıklama olaylarını bağla
        buttonPrimary.onClick.AddListener(LoadPrimaryScene);
        buttonPreschool.onClick.AddListener(LoadPreschoolScene);
    }

    void LoadPrimaryScene()
    {
        SceneManager.LoadScene("PrimaryModeScene");
    }

    void LoadPreschoolScene()
    {
        SceneManager.LoadScene("PreschoolModeScene");
    }
}
