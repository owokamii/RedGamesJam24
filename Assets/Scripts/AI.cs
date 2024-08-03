using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool isFacingRight;
    [SerializeField] private float pauseDuration; // Duration of the pause at each destination

    private Vector2 waypoint;
    private SpriteRenderer spriteRenderer;
    private bool isMoving = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewDestination();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveTowardsWaypoint();
        }

        FlipSprite();
    }

    private void MoveTowardsWaypoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);

        Vector2 direction = waypoint - (Vector2)transform.position;

        if (direction.x > 0)
        {
            isFacingRight = true;
        }
        else if (direction.x < 0)
        {
            isFacingRight = false;
        }

        // Check if the AI has reached the waypoint
        if (Vector2.Distance(transform.position, waypoint) < 0.1f)
        {
            StartCoroutine(PauseBeforeNextMove());
        }
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = isFacingRight;
    }

    private void SetNewDestination()
    {
        waypoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }

    private IEnumerator PauseBeforeNextMove()
    {
        isMoving = false;
        yield return new WaitForSeconds(pauseDuration);
        SetNewDestination();
        isMoving = true;
    }
}