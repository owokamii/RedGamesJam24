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
    private bool hasScored;

    private SpriteChanger spriteChanger;
    private RandomSpawner randomSpawner;
    private Sprite[] currentSprites;
    public int currentSpriteIndex;
    private Sprite initialSprite;
    private int spawnPointIndex;
    public float remainingDestroyTime = -1f;

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Basket").transform;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteChanger = FindObjectOfType<SpriteChanger>();
        randomSpawner = FindObjectOfType<RandomSpawner>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            initialSprite = spriteRenderer.sprite;
        }

        spawnPointIndex = GetSpawnPointIndex();

        // Example of setting current sprites, you should replace someSpriteArray with your actual sprite array
        SetCurrentSprites(spriteChanger.plantSprites, 0);
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

            if (newScale.y >= initialScale.y * maxStretchScale)
            {
                FindObjectOfType<AudioManager>().PlaySFX("POP");
                isMoving = true;
                capsuleCollider.enabled = false;
                spriteChanger.StopCoroutineForObject(gameObject);
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
        if (!hasScored)
        {
            CheckPlantStatus();
            hasScored = true;
        }
        DestroyAndReset();
    }

    private void DestroyAndReset()
    {
        if (spawnPointIndex != -1 && randomSpawner != null)
        {
            spriteChanger.CheckingColliderEnter();
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

            if (distance < 0.6f)
            {
                return i;
            }
        }
        return -1;
    }

    //加进来新object资料
    public void CheckPlantStatus()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && currentSprites != null && currentSpriteIndex >= 0 && currentSpriteIndex < currentSprites.Length)
        {
            if (spriteRenderer.sprite != initialSprite)
            {
                if (gameObject.name.Contains("Plant"))
                {
                    if (currentSpriteIndex == 0)
                    {
                        GameManager.Instance.AddMoney(1);
                        GameManager.Instance.AddScore(100);
                        //Debug.Log("AAAAAAAAAAAAAAAAAAAAAA");
                    }
                    else if (currentSpriteIndex == 1)
                    {
                        GameManager.Instance.AddMoney(0);
                        GameManager.Instance.AddScore(50);
                        //Debug.Log("BBBBBBBBBBBBBBBBBBBBB");
                    }
                }
                else if (gameObject.name.Contains("Circle"))
                {
                    if (currentSpriteIndex == 0)
                    {
                        GameManager.Instance.AddMoney(0);
                        GameManager.Instance.AddScore(50);
                        Debug.Log("CCCCCCCCCCCCCCCCCCCC");
                    }
                    else if (currentSpriteIndex == 1)
                    {
                        GameManager.Instance.AddMoney(1);
                        GameManager.Instance.AddScore(100);
                        Debug.Log("DDDDDDDDDDDDDDDDDDDD");
                    }
                }
                else if (gameObject.name.Contains("Plant3"))
                {
                    if (currentSpriteIndex == 0)
                    {
                        GameManager.Instance.AddMoney(0);
                        GameManager.Instance.AddScore(50);
                        Debug.Log("CCCCCCCCCCCCCCCCCCCC");
                    }
                    else if (currentSpriteIndex == 1)
                    {
                        GameManager.Instance.AddMoney(1);
                        GameManager.Instance.AddScore(100);
                        Debug.Log("DDDDDDDDDDDDDDDDDDDD");
                    }
                }
            }
        }
    }

    public void SetCurrentSprites(Sprite[] sprites, int startIndex)
    {
        currentSprites = sprites;
        currentSpriteIndex = startIndex;
        //Debug.Log("SetCurrentSprites called with startIndex: " + startIndex);
    }

    public void UpdateCurrentSpriteIndex(int newIndex)
    {
        currentSpriteIndex = newIndex;
        //Debug.Log($"Updated currentSpriteIndex to {currentSpriteIndex} for {gameObject.name}");
    }
}
