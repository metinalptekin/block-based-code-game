using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource; // Audio Source referansı
    public Slider musicSlider;      // Slider referansı

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.volume = 0.4f; // Başlangıç sesi %40
            musicSource.Play();
        }

        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.value = 0.4f;
            musicSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void SetVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }
}
