using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float minRoamTime = 1.0f;
    [SerializeField] private float maxRoamTime = 3.0f;
    [SerializeField] private float minIdleTime = 0.5f;
    [SerializeField] private float maxIdleTime = 1.5f;
    [SerializeField] private Animator animator; // Reference to the Animator component

    private Draggable draggable;
    private Vector2 direction;
    public bool isMoving;
    private float roamTime;
    private float idleTime;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private float screenLeft;
    private float screenRight;

    private void Start()
    {
        draggable = GetComponent<Draggable>();
        animator = GetComponent<Animator>(); // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        // Get the screen boundaries in world coordinates
        screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        ChooseNewDirection();
    }

    private void Update()
    {
        if (draggable.isGrounded)
        {
            if (isMoving)
            {
                MoveInDirection();
            }
            else
            {
                // Check if idle time is over to start moving again
                if (idleTime <= 0)
                {
                    ChooseNewDirection();
                }
                else
                {
                    idleTime -= Time.deltaTime; // Decrease idleTime when AI is idle
                }
            }
        }
        else
        {
            isMoving = false;  // Stop moving when not grounded
            animator.SetBool("IsWalking", false); // Stop the walking animation
        }
    }

    private void MoveInDirection()
    {
        // Calculate new position
        Vector3 newPosition = transform.position + (Vector3)(direction * speed * Time.deltaTime);

        // Check if the new position is within the screen boundaries
        if (newPosition.x >= screenLeft && newPosition.x <= screenRight)
        {
            transform.position = newPosition;
        }
        else
        {
            // If out of boundary, reverse direction
            direction = -direction;
            newPosition = transform.position + (Vector3)(direction * speed * Time.deltaTime);

            // Ensure the new position is within boundaries before applying
            if (newPosition.x >= screenLeft && newPosition.x <= screenRight)
            {
                transform.position = newPosition;
            }
            else
            {
                // If still out of boundary, stop moving and choose a new direction
                StartCoroutine(Idle());
                return;
            }
        }

        animator.SetBool("IsWalking", true); // Start the walking animation

        // Flip the sprite based on the direction
        if (direction == Vector2.left)
        {
            spriteRenderer.flipX = true; // Face left
        }
        else if (direction == Vector2.right)
        {
            spriteRenderer.flipX = false; // Face right
        }

        roamTime -= Time.deltaTime;
        if (roamTime <= 0)
        {
            StartCoroutine(Idle());
        }
    }

    private IEnumerator Idle()
    {
        isMoving = false;
        animator.SetBool("IsWalking", false); // Stop the walking animation
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        ChooseNewDirection();
    }

    private void ChooseNewDirection()
    {
        isMoving = true;
        roamTime = Random.Range(minRoamTime, maxRoamTime);

        int randomDirection = Random.Range(0, 2);
        switch (randomDirection)
        {
            case 0:
                direction = Vector2.left;
                break;
            case 1:
                direction = Vector2.right;
                break;
        }
    }
}