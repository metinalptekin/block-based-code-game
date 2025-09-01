using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public int id;
    public bool isBlocked = false;

    public TextMeshPro numberText;
    public Vector3 numberLocalOffset = new Vector3(0f, 0f, -0.02f);
    public int labelSortingOrderOffset = 1;

    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    private Vector3 originalScale;

    private static readonly Color[] obstacleColors = {
        Color.red, Color.green, Color.blue, Color.yellow,
        new Color(1f, 0.5f, 0f), Color.magenta, Color.cyan
    };
    private static int obstacleColorIndex = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        originalScale = transform.localScale;

        if (numberText != null)
        {
            numberText.transform.localPosition = numberLocalOffset;
            var mr = numberText.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.sortingLayerID = spriteRenderer.sortingLayerID;
                mr.sortingOrder   = spriteRenderer.sortingOrder + labelSortingOrderOffset;
            }
        }
    }

    public void SetCoordinates(int x, int y) { this.x = x; this.y = y; }

    public void SetNumber(int number)
    {
        id = number;
        if (numberText != null)
        {
            numberText.text = number.ToString();
            var mr = numberText.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.sortingLayerID = spriteRenderer.sortingLayerID;
                mr.sortingOrder   = spriteRenderer.sortingOrder + labelSortingOrderOffset;
            }
        }
    }

    public void BlockTile() { isBlocked = true; SetAsObstacle(); }

    public void SetAsNormal()
    {
        isBlocked = false;
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = originalSprite;
        transform.localScale = originalScale;
    }

    public void SetAsStart()
    {
        spriteRenderer.color = Color.white;
        SetSpriteFromResources("HomeSprite", "HomeSprite.png");
    }

    public void SetAsGoal(Sprite customSprite = null)
    {
        spriteRenderer.color = Color.white;
        if (customSprite != null) SetSpriteWithAutoScaleToOriginal(customSprite);
        else SetSpriteFromResources("TargetSprite", "TargetSprite.png");
    }

    public void SetAsObstacle()
    {
        spriteRenderer.color = obstacleColors[obstacleColorIndex % obstacleColors.Length];
        obstacleColorIndex++;
        if (!SetSpriteFromResources("ObstacleSprite", "ObstacleSprite.png"))
        {
            spriteRenderer.sprite = originalSprite;
            transform.localScale = originalScale;
        }
    }

    private bool SetSpriteFromResources(string resourceName, string errorLogName)
    {
        Sprite sprite = Resources.Load<Sprite>(resourceName);
        if (sprite != null) { SetSpriteWithAutoScaleToOriginal(sprite); return true; }
        Debug.LogError($"{errorLogName} bulunamadı! Lütfen Resources klasörüne yerleştir.");
        return false;
    }

    private void SetSpriteWithAutoScaleToOriginal(Sprite newSprite)
    {
        if (newSprite == null) return;
        spriteRenderer.sprite = newSprite;
        Vector2 s = spriteRenderer.sprite.bounds.size;
        transform.localScale = new Vector3(originalScale.x / s.x, originalScale.y / s.y, 1f);
    }
}
