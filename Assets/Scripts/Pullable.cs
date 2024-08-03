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
    public bool isDestroyed; // 标志对象是否已被销毁

    private SpriteChanger spriteChanger;
    private Sprite[] currentSprites;
    public int currentSpriteIndex;

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Basket").transform;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteChanger = FindObjectOfType<SpriteChanger>();
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
        ShrinkObject();
    }

    private void ShrinkObject()
    {
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        if (transform.localScale.x < 0 || transform.localScale.y < 0 || transform.localScale.z < 0)
        {
            transform.localScale = Vector3.zero;
            CheckPlantStatus();
            isDestroyed = true; // 设置标志位，表示对象已被销毁
            Destroy(gameObject);
        }
    }

    public void CheckPlantStatus()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (gameObject.name.Contains("Plant1"))
            {
                if (spriteRenderer.sprite == currentSprites[0])
                {
                    GameManager.Instance.AddMoney(0);
                    GameManager.Instance.AddScore(50);
                }
                else if (spriteRenderer.sprite == currentSprites[1])
                {
                    GameManager.Instance.AddMoney(1);
                    GameManager.Instance.AddScore(100);
                }
                else if (spriteRenderer.sprite == currentSprites[2])
                {
                    GameManager.Instance.AddMoney(0);
                    GameManager.Instance.AddScore(25);
                }
            }
            else if (gameObject.name.Contains("Plant2"))
            {
                if (spriteRenderer.sprite == currentSprites[0])
                {
                    GameManager.Instance.AddMoney(0);
                    GameManager.Instance.AddScore(50);
                }
                else if (spriteRenderer.sprite == currentSprites[1])
                {
                    GameManager.Instance.AddMoney(1);
                    GameManager.Instance.AddScore(100);
                }
                else if (spriteRenderer.sprite == currentSprites[2])
                {
                    GameManager.Instance.AddMoney(0);
                    GameManager.Instance.AddScore(25);
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
