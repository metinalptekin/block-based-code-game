using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class BlocksHeaderTextLocalizer : EditorWindow
{
    [MenuItem("Tools/Blok Yazılarını Türkçeleştir")]
    public static void LocalizeBlockTexts()
    {
        string blocksFolderPath = "Assets/BlocksEngine2/Prefabs/Block";
        string[] prefabPaths = Directory.GetFiles(blocksFolderPath, "*.prefab", SearchOption.AllDirectories);

        Dictionary<string, string> translations = new Dictionary<string, string>()
        {
            { "When clicked", "Tıklandığında" },
            { "Move", "İleri" },
            { "Turn Left", "Sola Dön" },
            { "Turn Right", "Sağa Dön" },
            { "Repeat", "Tekrar" },
            { "Wait", "Bekle" },
            { "If", "Eğer" },
            { "Else", "Değilse" },
            { "When", "Olduğunda" },
            { "clicked", "tıklanırsa" },
            { "Key", "Tuş" }
            // Gerekirse buraya daha fazlasını ekle
        };

        int changedCount = 0;

        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            TMP_Text[] texts = prefab.GetComponentsInChildren<TMP_Text>(true);
            bool changed = false;

            foreach (TMP_Text text in texts)
            {
                string original = text.text;
                foreach (var kvp in translations)
                {
                    if (original.Contains(kvp.Key))
                    {
                        original = original.Replace(kvp.Key, kvp.Value);
                        changed = true;
                    }
                }

                if (changed)
                    text.text = original;
            }

            if (changed)
            {
                PrefabUtility.SavePrefabAsset(prefab);
                changedCount++;
            }
        }

        Debug.Log($"{changedCount} blok prefabı Türkçeleştirildi.");
        AssetDatabase.Refresh();
    }
}
