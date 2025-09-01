using UnityEngine;
using UnityEngine.UI;

public class LobbyTutorialManager : MonoBehaviour
{
    [Header("Referanslar")]
    public GameObject tutorialFocusPanel; // Lobby_Tutorial_Focus_common
    public Button shopButton;             // Button_Shop
    public Button cancelButton;           // Info_cancel

    void Start()
    {
        // Butonlara tıklama eventlerini bağla
        shopButton.onClick.AddListener(OpenTutorial);
        cancelButton.onClick.AddListener(CloseTutorial);

        // Başlangıçta tutorial panel kapalıysa pasif yap
        if (tutorialFocusPanel != null)
            tutorialFocusPanel.SetActive(false);
            Debug.Log("Start çağrıldı");
    Debug.Log("Cancel button: " + (cancelButton != null));
    }

    void OpenTutorial()
    {
        tutorialFocusPanel.SetActive(true);
    }

    public void CloseTutorial()
{
    Debug.Log("Info_cancel butonuna basıldı.");
    tutorialFocusPanel.SetActive(false);
}

}
