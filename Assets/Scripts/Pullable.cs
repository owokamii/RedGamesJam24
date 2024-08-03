using UnityEngine;

public class Pullable : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float maxStretchScale = 2f;
    [SerializeField] private float shrinkSpeed = 3f;
    [SerializeField] private float moveSpeed = 10f;

    private CapsuleCollider2D capsuleCollider;
    private Vector3 initialMousePosition;
    private Vector3 initialScale;
    public bool isMoving;
    public bool isBeingDragged;
    public bool isDestroyed;
    private bool hasScored; // 标志对象是否已经计算过分数

    private SpriteChanger spriteChanger;
    private RandomSpawner randomSpawner;
    private Sprite[] currentSprites;
    public int currentSpriteIndex;
    private Sprite initialSprite; // 保存初始 Sprite
    private int spawnPointIndex; // 记录生成点索引
    public float remainingDestroyTime = -1f; // 剩余销毁时间

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Basket").transform;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteChanger = FindObjectOfType<SpriteChanger>();
        randomSpawner = FindObjectOfType<RandomSpawner>();

        // 获取初始 Sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            initialSprite = spriteRenderer.sprite;
        }

        // 获取生成点索引
        spawnPointIndex = GetSpawnPointIndex();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveObject();

            if (Vector3.Distance(transform.position, targetTransform.position) < 0.01f)
            {
                StopObject();
            }
        }
    }

    private void OnMouseDown()
    {
        initialScale = transform.localScale;
        initialMousePosition = Input.mousePosition;
        isBeingDragged = true;
    }

    private void OnMouseDrag()
    {
        StretchObject();
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        ResetScale();
        spriteChanger.RestartCoroutine(gameObject, currentSprites, currentSpriteIndex);
    }

    private void ResetScale()
    {
        transform.localScale = initialScale;
    }

    private void StretchObject()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        float distance = currentMousePosition.y - initialMousePosition.y;

        if (distance > 0)
        {
            float stretchFactor = distance / Screen.height;
            Vector3 newScale = new Vector3(initialScale.x, initialScale.y + stretchFactor, initialScale.z);

            transform.localScale = newScale;

            // 完成拉伸
            if (newScale.y >= initialScale.y * maxStretchScale)
            {
                isMoving = true;
                capsuleCollider.enabled = false;
                spriteChanger.StopCoroutineForObject(gameObject); // 停止更换Sprite的协程
                ResetScale();
            }
        }
    }

    private void MoveObject()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
    }

    private void StopObject()
    {
        if (!hasScored) // 确保每个对象只会计算一次分数
        {
            CheckPlantStatus();
            hasScored = true;
        }
        DestroyAndReset(); // 立即销毁对象并重置生成点
    }

    private void ShrinkObject()
    {
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        if (transform.localScale.x < 0 || transform.localScale.y < 0 || transform.localScale.z < 0)
        {
            transform.localScale = Vector3.zero;
            isDestroyed = true;
            DestroyAndReset(); // 销毁对象并重置生成点
        }
    }

    private void DestroyAndReset()
    {
        if (spawnPointIndex != -1 && randomSpawner != null)
        {
            randomSpawner.ResetSpawnPoint(spawnPointIndex);
        }
        Destroy(gameObject);
    }

    private int GetSpawnPointIndex()
    {
        if (randomSpawner == null) return -1;

        Vector3 objPosition = transform.position;

        for (int i = 0; i < randomSpawner.spawnPoints.Length; i++)
        {
            Vector3 spawnPointPosition = randomSpawner.spawnPoints[i].transform.position;
            float distance = Vector3.Distance(spawnPointPosition, objPosition);

            if (distance < 0.5f)
            {
                return i;
            }
        }
        return -1;
    }

    public void CheckPlantStatus()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && currentSprites != null && currentSpriteIndex >= 0 && currentSpriteIndex < currentSprites.Length)
        {
            // 确保只有在 Sprite 已经被更换时才会加分
            if (spriteRenderer.sprite != initialSprite)
            {
                if (gameObject.name.Contains("Plant"))
                {
                    if (currentSpriteIndex == 0)
                    {
                        GameManager.Instance.AddMoney(1);
                        GameManager.Instance.AddScore(50);
                    }
                }
                else if (gameObject.name.Contains("Plant2"))
                {
                    if (currentSpriteIndex == 1)
                    {
                        GameManager.Instance.AddMoney(1);
                        GameManager.Instance.AddScore(100);
                    }
                    else if (currentSpriteIndex == 2)
                    {
                        GameManager.Instance.AddMoney(0);
                        GameManager.Instance.AddScore(25);
                    }
                }
            }
        }
    }

    public void SetCurrentSprites(Sprite[] sprites, int startIndex)
    {
        currentSprites = sprites;
        currentSpriteIndex = startIndex;
    }
}
