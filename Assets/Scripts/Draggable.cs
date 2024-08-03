using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float gravityScale = 10f;
    [SerializeField] private float groundY = -20f;

    private AI ai;
    private Vector2 difference = Vector2.zero;
    private Vector3 velocity = Vector3.zero;
    private bool isDragged = false;
    public bool isGrounded = true;

    private void Start()
    {
        ai = GetComponent<AI>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDragged)
        {
            velocity.y -= gravityScale * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;

            if (transform.position.y <= groundY)
            {
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
                velocity.y = 0;
                isGrounded = true;
            }
            else if(transform.position.y >= groundY)
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnMouseDown()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        animator.SetBool("IsDragged", true);
        isDragged = true;
        velocity = Vector3.zero;
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void OnMouseUp()
    {
        animator.SetBool("IsDragged", false);
        isDragged = false;
    }
}