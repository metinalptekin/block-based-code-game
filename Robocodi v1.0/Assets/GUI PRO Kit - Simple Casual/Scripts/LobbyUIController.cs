using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public Button stageButton; // Button_Stage
    public GameObject stageListPanel; // Stage_List_Blue_common

    void Start()
    {
        stageButton.onClick.AddListener(ShowStageList);
    }

    void ShowStageList()
    {
        stageListPanel.SetActive(true);
    }
}
