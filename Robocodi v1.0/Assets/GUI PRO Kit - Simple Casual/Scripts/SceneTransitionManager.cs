using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public GameObject titleScreen;        // Title_common objesi
    public GameObject loadingScreen;      // LoadingBar_common objesi
    public Button startButton;            // Buton objesi

    void Start()
    {
        // Butona dinleyici ekle
        startButton.onClick.AddListener(OnStartButtonClicked);

        // Başlangıçta loading ekranı kapalıysa kontrol edelim
        loadingScreen.SetActive(false);
    }

    void OnStartButtonClicked()
    {
        // Title_common kapanıyor
        titleScreen.SetActive(false);

        // LoadingBar_common açılıyor
        loadingScreen.SetActive(true);
    }
}
