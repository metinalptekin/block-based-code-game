using UnityEngine;
using UnityEngine.UI;

public class MapsPanelController : MonoBehaviour
{
    public GameObject lobbyMapsMessage; // Lobby_Maps_Message objesi
    public Button buttonMaps;           // Button_Maps
    public Button buttonExit;           // Button_Exit

    void Start()
    {
        // İlk durumda kapalı
        lobbyMapsMessage.SetActive(false);

        // Butonlara tıklama eventleri ekle
        buttonMaps.onClick.AddListener(OpenMapsMessage);
        buttonExit.onClick.AddListener(CloseMapsMessage);
    }

    void OpenMapsMessage()
    {
        lobbyMapsMessage.SetActive(true);
    }

    void CloseMapsMessage()
    {
        lobbyMapsMessage.SetActive(false);
    }
}
