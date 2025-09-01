using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using MG_BlocksEngine2.Environment;

public class GridManager : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject tilePrefab;
    public float tileSize = 1f;
    public float spacing = 0.1f;

    public LevelData[] levels;
    public int currentLevelIndex = 0;

    public GameObject winPanel;
    public TextMeshProUGUI objectiveText;

    public BE2_TargetObjectSpacecraft3D player;

    public bool numberedObjectiveMode = true;
    public bool noObstaclesInThisMode = true;

    [HideInInspector] public Tile[,] tiles;
    [HideInInspector] public Tile startTile;
    [HideInInspector] public Tile targetTile;

    private Vector2Int startPosition;
    private Vector2Int targetPosition;
    private Vector2Int[] blockedTiles;
    private Vector2 gridOffset;
    private List<Tile> allTiles = new List<Tile>();
    private LevelData currentLevelData;
    private System.Random rng = new System.Random();

    public Action<int> OnLevelChanged;

    void Start()
    {
        CalculateGridOffset();
        LoadLevel(currentLevelIndex);
    }

    private void CalculateGridOffset()
    {
        gridOffset = new Vector2(
            -(width - 1) * (tileSize + spacing) / 2f + 7f,
            -(height - 1) * (tileSize + spacing) / 2f + 1f
        );
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length) return;

        currentLevelData = levels[levelIndex];
        width = currentLevelData.gridWidth;
        height = currentLevelData.gridHeight;
        startPosition = currentLevelData.startPosition;

        if (currentLevelData.system == LevelData.SystemType.MetinSistemi)
        {
            blockedTiles = currentLevelData.blockedTiles;
            targetPosition = currentLevelData.targetPosition;
        }
        else
        {
            blockedTiles = Array.Empty<Vector2Int>();
            targetPosition = new Vector2Int(-999, -999);
        }

        GenerateGrid();
        PositionCamera();

        if (player != null && startTile != null)
        {
            player.gridManager = this;
            player.transform.position = startTile.transform.position;
            player.currentX = startTile.x;
            player.currentY = startTile.y;
            player.direction = Vector2Int.up;
            player.transform.rotation = Quaternion.identity;
        }

        if (currentLevelData.system == LevelData.SystemType.YeniSistem && numberedObjectiveMode)
        {
            PickRandomTargetTile();
        }

        winPanel?.SetActive(false);
        OnLevelChanged?.Invoke(levelIndex + 1);
    }

    private void GenerateGrid()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);

        tiles = new Tile[width, height];
        allTiles.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x * (tileSize + spacing), y * (tileSize + spacing)) + gridOffset;
                GameObject tileObj = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tileObj.transform.localScale = Vector3.one * tileSize;

                Tile tile = tileObj.GetComponent<Tile>();
                tile.SetCoordinates(x, y);
                tile.SetNumber((y * width) + x + 1);

                bool placeStart = startPosition == new Vector2Int(x, y);
                bool placeTarget = currentLevelData.system == LevelData.SystemType.MetinSistemi && targetPosition == new Vector2Int(x, y);
                bool placeBlocked = currentLevelData.system == LevelData.SystemType.MetinSistemi && IsBlocked(x, y);

                if (placeStart) { tile.SetAsStart(); startTile = tile; }
                else if (placeTarget) { tile.SetAsGoal(currentLevelData.targetSprite); targetTile = tile; }
                else if (placeBlocked) { tile.BlockTile(); }
                else { tile.SetAsNormal(); }

                tiles[x, y] = tile;
                allTiles.Add(tile);
            }
        }
    }

    private bool IsBlocked(int x, int y)
    {
        foreach (var b in blockedTiles) if (b.x == x && b.y == y) return true;
        return false;
    }

    public Tile GetTileAt(int x, int y) => (x >= 0 && y >= 0 && x < width && y < height) ? tiles[x, y] : null;

    private void PositionCamera()
    {
        Camera.main.transform.position = new Vector3(
            (width - 1) * (tileSize + spacing) / 2f,
            (height - 1) * (tileSize + spacing) / 2f - 1.5f,
            -10f
        );
    }

    private void PickRandomTargetTile()
    {
        List<Tile> candidates = new List<Tile>();
        foreach (var t in allTiles)
        {
            if (t != null && t != startTile && !t.isBlocked) candidates.Add(t);
        }

        if (candidates.Count == 0) return;

        int idx = rng.Next(candidates.Count);
        var chosen = candidates[idx];

        if (targetTile != null && targetTile != startTile) targetTile.SetAsNormal();

        targetTile = chosen;
        targetTile.SetAsGoal(currentLevelData.targetSprite);

        if (objectiveText != null) objectiveText.text = "Evinize gidin";
    }

    public void PlayerArrivedAt(int gridX, int gridY)
    {
        var t = GetTileAt(gridX, gridY);
        if (t == targetTile) winPanel?.SetActive(true);
    }

    public void NextLevel()
    {
        if (currentLevelIndex < levels.Length - 1)
        {
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
            player?.ResetPositionForNextLevel();
        }
    }

    public void PreviousLevel()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
            LoadLevel(currentLevelIndex);
        }
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
        player?.ResetToStartPosition();
    }

    public void ShowWinPanel() => winPanel?.SetActive(true);
}
