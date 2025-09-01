using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public enum SystemType
    {
        MetinSistemi,
        YeniSistem
    }

    [Header("Sistem Tipi")]
    public SystemType system = SystemType.MetinSistemi;

    [Header("Grid Boyutu")]
    public int gridWidth = 5;
    public int gridHeight = 5;

    [Header("Metin'in Sistemi")]
    public Vector2Int startPosition = new Vector2Int(0, 0);
    public Vector2Int targetPosition = new Vector2Int(4, 4);
    public Vector2Int[] blockedTiles = Array.Empty<Vector2Int>();

    [Header("Hedef Sprite")]
    public Sprite targetSprite; // <-- Yeni alan, Inspector üzerinden atanacak

    [Header("Yeni Sistem")]
    public bool randomizeTargetOnPlay = true;
    public Vector2Int fixedTargetOverride = new Vector2Int(-1, -1);
    public int seed = 0;

    [SerializeField, HideInInspector] 
    private int _lastGeneratedTargetId = -1;

    public void EnsurePrepared()
    {
        if (system == SystemType.YeniSistem)
            PrepareYeniSistem();
    }

    private void PrepareYeniSistem()
    {
        blockedTiles = Array.Empty<Vector2Int>();

        Vector2Int chosen;

        if (fixedTargetOverride.x >= 0 && fixedTargetOverride.y >= 0)
        {
            chosen = fixedTargetOverride;
        }
        else
        {
            int w = Mathf.Max(1, gridWidth);
            int h = Mathf.Max(1, gridHeight);
            int total = w * h;

            System.Random rnd = (seed == 0)
                ? new System.Random(Environment.TickCount + GetInstanceID())
                : new System.Random(seed + GetInstanceID());

            int startId = ToId(startPosition.x, startPosition.y, w);
            int targetId;
            do
            {
                targetId = rnd.Next(0, total);
            } while (targetId == startId);

            _lastGeneratedTargetId = targetId;
            chosen = ToXY(targetId, w);
        }

        targetPosition = chosen;
    }

    public static int ToId(int x, int y, int width) => y * width + x;

    public static Vector2Int ToXY(int id, int width)
    {
        int x = id % width;
        int y = id / width;
        return new Vector2Int(x, y);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (system == SystemType.YeniSistem && !Application.isPlaying)
        {
            PrepareYeniSistem();
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
