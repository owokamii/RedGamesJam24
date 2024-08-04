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

    private Animator animator;
    private GrowthStages growthStages;
    private AnimationChanger animationChanger;
    private RandomSpawner randomSpawner;
    public string[] animationStages;
    public int currentAnimationStageIndex;
    private int spawnPointIndex;
    public float remainingDestroyTime = -1f;
    public bool hasChangedState = false;

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Basket").transform;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animationChanger = FindObjectOfType<AnimationChanger>();
        randomSpawner = FindObjectOfType<RandomSpawner>();
        animator = GetComponent<Animator>();
        growthStages = GetComponent<GrowthStages>();

        spawnPointIndex = GetSpawnPointIndex();

        animationStages = new string[] { "Stage1", "Stage2", "Stage3" };

        SetCurrentAnimationStage(0);
    }

    private void Update()
    {
        Debug.Log(isBeingDragged);
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
        animationChanger.RestartCoroutine(gameObject, growthStages.GetCurrentStage());
    }

    private void OnMouseDrag()
    {
        StretchObject();
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        ResetScale();
        animationChanger.RestartCoroutine(gameObject, growthStages.GetCurrentStage());
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
                animationChanger.StopCoroutineForObject(gameObject);
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
            CheckAnimationStatus();
            hasScored = true;
        }
        ShrinkObject();
    }

    private void ShrinkObject()
    {
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        if (transform.localScale.x < 0 || transform.localScale.y < 0 || transform.localScale.z < 0)
        {
            DestroyAndReset();
        }
    }

    private void DestroyAndReset()
    {
        if (spawnPointIndex != -1 && randomSpawner != null)
        {
            animationChanger.CheckingColliderEnter();
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

    public void CheckAnimationStatus()
    {
        int currentStage = growthStages.GetCurrentStage();
        if (gameObject.name.Contains("Plant"))
        {
            if (currentStage == 0)
            {
                GameManager.Instance.AddMoney(0);
                GameManager.Instance.AddScore(0);
            }
            else if (currentStage == 1)
            {
                GameManager.Instance.AddMoney(1);
                GameManager.Instance.AddScore(100);
            }
            else if (currentStage == 2)
            {
                GameManager.Instance.AddMoney(0);
                GameManager.Instance.AddScore(50);
            }
        }
        else if (gameObject.name.Contains("Circle"))
        {
            if (currentStage == 0)
            {
                GameManager.Instance.AddMoney(0);
                GameManager.Instance.AddScore(50);
            }
            else if (currentStage == 1)
            {
                GameManager.Instance.AddMoney(1);
                GameManager.Instance.AddScore(100);
            }
        }
        else if (gameObject.name.Contains("Plant3"))
        {
            if (currentStage == 0)
            {
                GameManager.Instance.AddMoney(0);
                GameManager.Instance.AddScore(50);
            }
            else if (currentStage == 1)
            {
                GameManager.Instance.AddMoney(1);
                GameManager.Instance.AddScore(100);
            }
        }
    }

    public void SetCurrentAnimationStage(int stageIndex)
    {
        currentAnimationStageIndex = stageIndex;
        animator.Play(animationStages[stageIndex]);
    }

    public void UpdateCurrentAnimationStage(int newIndex)
    {
        currentAnimationStageIndex = newIndex;
    }
}
