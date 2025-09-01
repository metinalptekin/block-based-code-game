using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class BlockNameTranslator : EditorWindow
{
    private static Dictionary<string, string> translationMap = new Dictionary<string, string>()
    {
        { "Block Move", "Blok İleri" },
        { "Block TurnLeft", "Blok Sola Dön" },
        { "Block TurnRight", "Blok Sağa Dön" },
        { "Block If", "Blok Eğer" },
        { "Block Else", "Blok Değilse" },
        { "Block Repeat", "Blok Tekrar" },
        { "Block Wait", "Blok Bekle" }
        // ihtiyaca göre ekle
    };

    [MenuItem("Tools/Blokları Türkçeleştir")]
    public static void TranslateBlocks()
    {
        string blockPath = "Assets/BlocksEngine2/Prefabs/Block/";

        foreach (string path in Directory.GetFiles(blockPath, "*.prefab"))
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            string originalName = prefab.name;

            if (translationMap.TryGetValue(originalName, out string newName))
            {
                // Prefab'ı Türkçeleştirilmiş isimle yeniden adlandır
                string newPath = Path.GetDirectoryName(path) + "/" + newName + ".prefab";
                AssetDatabase.RenameAsset(path, newName);
                Debug.Log($"Prefab adı değiştirildi: {originalName} → {newName}");

                // TMP_Text varsa onu da güncelle
                TMP_Text[] texts = prefab.GetComponentsInChildren<TMP_Text>(true);
                foreach (var text in texts)
                {
                    if (text.text.Contains("Move")) text.text = "İleri";
                    else if (text.text.Contains("Left")) text.text = "Sola Dön";
                    else if (text.text.Contains("Right")) text.text = "Sağa Dön";
                    else if (text.text.Contains("If")) text.text = "Eğer";
                    else if (text.text.Contains("Else")) text.text = "Değilse";
                    else if (text.text.Contains("Repeat")) text.text = "Tekrar";
                    else if (text.text.Contains("Wait")) text.text = "Bekle";
                    // ihtiyaca göre çoğalt
                }

                // Prefab değişikliklerini uygula
                EditorUtility.SetDirty(prefab);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Tüm bloklar Türkçeleştirildi.");
    }
}
