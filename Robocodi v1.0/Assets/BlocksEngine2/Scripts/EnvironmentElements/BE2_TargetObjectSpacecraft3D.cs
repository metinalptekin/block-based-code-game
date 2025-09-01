using System.Collections;
using UnityEngine;

namespace MG_BlocksEngine2.Environment
{
    public class BE2_TargetObjectSpacecraft3D : BE2_TargetObject
    {
        public new Transform Transform => transform;

        [Header("Grid & Movement")]
        public GridManager gridManager;
        public int currentX = 0, currentY = 0;
        public Vector2Int direction = Vector2Int.up;
        public float tileSize = 1.1f;
        public bool isMoving = false;

        [Header("Audio")]
        public AudioSource blumpAudio;
        public AudioSource winAudio;

        private GameObject _bullet;

        void Awake()
        {
            _bullet = transform.Find("Bullet")?.gameObject;
        }

        void Start()
        {
            SnapToStart();
        }

        public void SnapToStart()
        {
            if (gridManager?.startTile == null)
            {
                Debug.LogWarning("SnapToStart: gridManager veya startTile null!");
                return;
            }

            var start = gridManager.startTile;
            SetPosition(start.transform.position, start.x, start.y);
        }

        private void SetPosition(Vector3 position, int x, int y)
        {
            transform.position = position;
            currentX = x;
            currentY = y;
            direction = Vector2Int.up;
            transform.rotation = Quaternion.identity;
            isMoving = false;
        }

        public IEnumerator MoveOneStepSmooth()
        {
            if (isMoving) yield break;

            int targetX = currentX + direction.x;
            int targetY = currentY + direction.y;

            Tile tile = gridManager?.GetTileAt(targetX, targetY);
            if (tile == null || tile.isBlocked)
            {
                yield return new WaitForSeconds(0.05f);
                yield break;
            }

            isMoving = true;
            Vector3 start = transform.position;
            Vector3 end = tile.transform.position;
            float duration = 0.25f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
            currentX = targetX;
            currentY = targetY;

            blumpAudio?.Play();

            if (gridManager?.targetTile != null &&
                currentX == gridManager.targetTile.x && currentY == gridManager.targetTile.y)
            {
                gridManager.ShowWinPanel();
                winAudio?.Play();
            }

            isMoving = false;
        }

        public IEnumerator TurnRightSmooth()
        {
            if (isMoving) yield break;
            isMoving = true;
            direction = new Vector2Int(direction.y, -direction.x);
            yield return null; // 1 frame bekleme
            isMoving = false;
        }

        public IEnumerator TurnLeftSmooth()
        {
            if (isMoving) yield break;
            isMoving = true;
            direction = new Vector2Int(-direction.y, direction.x);
            yield return null;
            isMoving = false;
        }

        public void ResetPositionForNextLevel()
        {
            if (gridManager?.startTile == null)
            {
                Debug.LogWarning("ResetPositionForNextLevel: gridManager veya startTile null!");
                return;
            }

            SetPosition(gridManager.startTile.transform.position,
                        gridManager.startTile.x,
                        gridManager.startTile.y);

            Debug.Log($"ResetPositionForNextLevel çağrıldı. Pozisyon: {transform.position}");
        }

        public void Shoot()
        {
            if (_bullet == null) return;

            GameObject newBullet = Instantiate(_bullet, _bullet.transform.position, Quaternion.identity);
            newBullet.SetActive(true);

            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb?.AddForce(transform.forward * 1000);

            StartCoroutine(DestroyBulletAfterDelay(newBullet));
        }

        private IEnumerator DestroyBulletAfterDelay(GameObject bullet)
        {
            yield return new WaitForSeconds(1f);
            Destroy(bullet);
        }

        public void ResetToStartPosition()
        {
            StopAllCoroutines();
            if (gridManager?.startTile == null)
            {
                Debug.LogWarning("ResetToStartPosition: gridManager veya startTile null!");
                return;
            }

            SetPosition(gridManager.startTile.transform.position,
                        gridManager.startTile.x,
                        gridManager.startTile.y);

            Debug.Log($"ResetToStartPosition çağrıldı. Pozisyon: {transform.position}");
        }
    }
}
