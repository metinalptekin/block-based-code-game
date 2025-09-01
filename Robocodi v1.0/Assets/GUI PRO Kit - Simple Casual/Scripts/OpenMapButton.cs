using UnityEngine;

public class OpenMapButton : MonoBehaviour
{
    public string mapsURL = "https://www.google.com/maps/search/?api=1&query=40.9783341,37.9037413";

    public void OpenMaps()
    {
        Debug.Log("Opening URL: " + mapsURL);
        Application.OpenURL(mapsURL);
    }
}
