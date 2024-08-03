using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float maxDistance;

    private Vector2 waypoint;
    private Camera mainCamera;
    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;

    private void Start()
    {
        mainCamera = Camera.main;
        CalculateScreenBounds();
        SetNewDestination();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoint) < range)
        {
            SetNewDestination();
        }
    }

    private void CalculateScreenBounds()
    {
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = mainCamera.aspect * halfHeight;

        screenLeft = -halfWidth;
        screenRight = halfWidth;
        screenTop = halfHeight;
        screenBottom = -halfHeight;
    }

    private void SetNewDestination()
    {
        float newX = Random.Range(screenLeft + maxDistance, screenRight - maxDistance);
        float newY = Random.Range(screenBottom + maxDistance, screenTop - maxDistance);
        waypoint = new Vector2(newX, newY);
    }
}