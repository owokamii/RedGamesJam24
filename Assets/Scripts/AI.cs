using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float minRoamTime = 1.0f;
    [SerializeField] private float maxRoamTime = 3.0f;
    [SerializeField] private float minIdleTime = 0.5f;
    [SerializeField] private float maxIdleTime = 1.5f;

    //private Rigidbody2D rb;
    private Vector2 direction;
    public bool isMoving;
    private float roamTime;
    private float idleTime;

    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();

        ChooseNewDirection();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveInDirection();
        }

        /*if (rb.velocity.y < -5.0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -5.0f);
        }*/


        Debug.Log(isMoving);
    }

    private void MoveInDirection()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        roamTime -= Time.deltaTime;
        if (roamTime <= 0)
        {
            if(isMoving)
            {
                Debug.Log("imi dle lol");
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
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = true;
    }
}