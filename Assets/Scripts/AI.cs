using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float minRoamTime = 1.0f;
    [SerializeField] private float maxRoamTime = 3.0f;
    [SerializeField] private float minIdleTime = 0.5f;
    [SerializeField] private float maxIdleTime = 1.5f;

    private Draggable draggable;
    private Vector2 direction;
    public bool isMoving;
    private float roamTime;
    private float idleTime;

    private void Start()
    {
        draggable = GetComponent<Draggable>();
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
            }
        }
        else
        {
            isMoving = false;  // Stop moving when not grounded
        }
    }

    private void MoveInDirection()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        roamTime -= Time.deltaTime;
        if (roamTime <= 0)
        {
            StartCoroutine(Idle());
        }
    }

    private IEnumerator Idle()
    {
        isMoving = false;
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
                /*case 2:
                        direction = Vector2.up;
                    break;
                case 3:
                    direction = Vector2.down;
                    break;*/
        }
    }
}