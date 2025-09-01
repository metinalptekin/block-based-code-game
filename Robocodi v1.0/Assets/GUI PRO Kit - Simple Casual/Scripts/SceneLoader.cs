using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // Inspector üzerinden bu alana Mod_Panel atanacak
    public GameObject modPanel;

    // OYNA butonuna bağlı olacak fonksiyon
    public void ShowModPanel()
    {
        if (modPanel != null)
        {
            modPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Mod_Panel objesi atanmamış!");
        }
    }
}
