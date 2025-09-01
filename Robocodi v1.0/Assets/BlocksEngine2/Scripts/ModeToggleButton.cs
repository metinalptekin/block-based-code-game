using UnityEngine;
using UnityEngine.UI;

public class ModeToggleButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            ModeManager.Instance.ToggleMode();
        });
    }
}
