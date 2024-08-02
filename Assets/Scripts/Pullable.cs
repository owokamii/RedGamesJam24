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

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Basket").transform;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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
            Destroy(gameObject);
        }
    }
}
