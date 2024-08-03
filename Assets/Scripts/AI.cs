using System.Collections;
using UnityEditor;
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
        if (isMoving)
        {
            MoveInDirection();
        }

        if (draggable.isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void MoveInDirection()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        roamTime -= Time.deltaTime;
        if (roamTime <= 0)
        {
            if(isMoving)
            {
                StartCoroutine(Idle());
            }
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

        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0:
                direction = Vector2.left;
                break;
            case 1:
                direction = Vector2.right;
                break;
            case 2:
                    direction = Vector2.up;
                break;
            case 3:
                direction = Vector2.down;
                break;
        }
    }
}