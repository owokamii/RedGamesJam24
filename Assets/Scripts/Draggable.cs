using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private AI ai;
    private Rigidbody2D rb;
    private Vector2 difference = Vector2.zero;

    private void Start()
    {
        ai = GetComponent<AI>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(rb.velocity.y);
    }

    private void OnMouseDown()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        animator.SetBool("IsDragged", true);
        ai.isMoving = false;
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    private void OnMouseUp()
    {
        animator.SetBool("IsDragged", false);
    }
}
