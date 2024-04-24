using System.Collections;
using UnityEngine;
using static PlayerManager;

[DisallowMultipleComponent, RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField, Min(0)] private float speed = 10f;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private LayerMask bombLayer;
    [SerializeField] private LayerMask explosionLayer;
    [SerializeField, Range(0, 100)] private int changeDirectionChance = 5;

    private Vector2 moveDirection = Vector2.right;
    private readonly float raycastDistance = 1f;
    private readonly float raycastBomb = 1.1f;
    private bool vertical = false;
    private bool delay = false;
    private bool isLive = true;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isLive) return;

        Move(moveDirection);

        CheckBomb(moveDirection);
        CheckExplosion();

        Vector2 pos = new(transform.position.x - Mathf.Floor(transform.position.x), transform.position.y - Mathf.Floor(transform.position.y));

        if (vertical ? (pos.y > 0.3f && pos.y < 0.7f) : (pos.x > 0.3f && pos.x < 0.7f) || delay) return;

        if (Random.Range(0, 100) <= changeDirectionChance)
        {
            if (!vertical)
            {
                if (CheckForObstacle(Vector2.up))
                {
                    ChangeDirection(Vector2.up);
                }
                else if (CheckForObstacle(Vector2.down))
                {
                    ChangeDirection(Vector2.down);
                }
            }
            else
            {
                if (CheckForObstacle(transform.localScale.x > 0 ? Vector2.left : Vector2.right))
                {
                    ChangeDirection(Vector2.left, Vector2.right, (transform.localScale.x > 0), false);
                }
            }
        }
    }

    private void ChangeDirection(Vector2 dir1, Vector2 dir2 = default, bool dirValue = true, bool verticalValue = true)
    {
        moveDirection = dirValue ? dir1 : dir2;
        transform.position = new(Mathf.Floor(transform.position.x) + 0.5f, Mathf.Floor(transform.position.y) + 0.5f, 0f);
        vertical = verticalValue;
        StartCoroutine(Delay(2f));
    }

    IEnumerator Delay(float waitTime = 0.3f)
    {
        delay = true;
        yield return new WaitForSeconds(waitTime);
        delay = false;
    }

    private void CheckBomb(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastBomb, bombLayer);

        Debug.DrawRay(transform.position, direction * raycastBomb, Color.green);

        if (hit.collider)
        {
            moveDirection *= -1;
            if (!vertical) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Move(Vector2 direction)
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private bool CheckForObstacle(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, collisionLayer);

        Debug.DrawRay(transform.position, direction * raycastDistance, Color.red, 1f);

        return hit.collider == null;
    }

    private void CheckExplosion()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.zero, 0f, explosionLayer);

        if (hit.collider)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            isLive = false;
            animator.CrossFade("Death", 0.1f);
            StartCoroutine(WaitForAnimation());
        }
    }

    IEnumerator WaitForAnimation()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & collisionLayer) != 0)
        {
            moveDirection *= -1;
            if (moveDirection == Vector2.left || moveDirection == Vector2.right) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
