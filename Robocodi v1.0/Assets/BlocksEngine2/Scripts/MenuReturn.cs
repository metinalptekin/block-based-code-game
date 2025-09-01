using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuReturn : MonoBehaviour
{
    // Butonun OnClick() event'ine bu metodu bağla
    public void ReturnToMenu()
    {
        // Sahneyi yükle
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("RobocodiGiris");
    }

    // Sahne yüklendikten sonra çalışacak
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name == "RobocodiGiris")
    {
        GameObject lobby = GameObject.Find("Canvas/LightMode/Lobby"); 
        if (lobby != null)
        {
            lobby.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Lobby objesi bulunamadı! Path doğru mu?");
        }
    }

    SceneManager.sceneLoaded -= OnSceneLoaded;
}

}
