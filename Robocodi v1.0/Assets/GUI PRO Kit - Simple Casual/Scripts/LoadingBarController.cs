using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingBarController : MonoBehaviour
{
    [Header("Referanslar")]
    public GameObject loadingBarObject;       // LoadingBar_common
    public Slider loadingSlider;              // Slider_LoadingBar_Green
    public TextMeshProUGUI loadingText;       // TMP yazı: Loading... %XX
    public GameObject lobbyObject;            // Lobby prefab

    [Header("Ayarlar")]
    public float fillDuration = 10f;
    public float waitAfterFull = 2f;

    void OnEnable()
    {
        StartCoroutine(FillLoadingBar());
    }

    IEnumerator FillLoadingBar()
    {
        float elapsed = 0f;

        while (elapsed < fillDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / fillDuration);
            loadingSlider.value = progress;
            UpdateSliderText(progress);

            yield return null;
        }

        // %100 yaz ve 2 saniye bekle
        loadingSlider.value = 1f;
        UpdateSliderText(1f);

        yield return new WaitForSeconds(waitAfterFull);

        loadingBarObject.SetActive(false);
        lobbyObject.SetActive(true);
    }

    void UpdateSliderText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100f);
        loadingText.text = $"Yükleniyor... %{percent}";
    }
}
