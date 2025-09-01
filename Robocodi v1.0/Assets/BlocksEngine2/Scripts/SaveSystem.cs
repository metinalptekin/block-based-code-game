using UnityEngine;

public static class SaveSystem
{
    private const string LAST_LEVEL_KEY = "last_level";

    /// <summary>
    /// Oyuncunun bitirdiği levelIndex’e 1 ekleyip (bir sonraki level)
    /// PlayerPrefs’e kaydeder.
    /// </summary>
    public static void SaveLevel(int levelIndex)
    {
        int nextLevel = levelIndex + 1;
        PlayerPrefs.SetInt(LAST_LEVEL_KEY, nextLevel);
        PlayerPrefs.Save();
        Debug.Log($"[SaveSystem] Saved nextLevelIndex = {nextLevel}");
    }

    /// <summary>
    /// Kayıtlı levelı döner (defaultValue değeriyle birlikte).
    /// </summary>
    public static int LoadLevel(int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(LAST_LEVEL_KEY, defaultValue);
    }

    /// <summary>
    /// Kayıt var mı diye kontrol eder.
    /// </summary>
    public static bool HasSave()
    {
        return PlayerPrefs.HasKey(LAST_LEVEL_KEY);
    }
}
