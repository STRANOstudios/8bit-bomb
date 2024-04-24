using System.Collections;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField, Min(0)] private float speed;
    [SerializeField, Min(0f)] private float raycastDistance = 0.5f;
    [SerializeField] private LayerMask obstacleLayer;

    private InputHandler inputHandler;
    private Animator animator;
    private string currentAnimation;

    private bool isLive = true;
    private bool isDeathAnimation = false;

    public delegate void PlayerDeath();
    public static event PlayerDeath Death;

    private void Start()
    {
        inputHandler = InputHandler.Instance;
        animator = GetComponentInChildren<Animator>();

        ChangeAnimation("Idle");
    }

    private void Update()
    {
        if (!isLive) return;

        HadlerMovement();
    }

    private void OnEnable()
    {
        ExplosionVFX.HitPlayer += Died;
        Health.HitEnemy += Died;
    }

    private void OnDisable()
    {
        ExplosionVFX.HitPlayer -= Died;
        Health.HitEnemy -= Died;
    }

    private void HadlerMovement()
    {
        Vector2 moveDirection = inputHandler.MoveInput.normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, raycastDistance, obstacleLayer);

        Debug.DrawRay(transform.position, moveDirection * raycastDistance, Color.red);

        if (!hit.collider)
        {
            transform.Translate(speed * Time.deltaTime * moveDirection);
        }

        MoveInDirection(moveDirection);
    }

    void MoveInDirection(Vector2 moveDirection)
    {
        if (moveDirection == Vector2.zero)
        {
            ChangeAnimation("Idle");
            return;
        }

        string animationName = "";

        float angle = Vector2.SignedAngle(Vector2.up, moveDirection.normalized);

        if (angle > -45f && angle < 45f)
        {
            animationName = "Backward";
        }
        else if (angle >= 45f && angle <= 135f)
        {
            animationName = "Left";
        }
        else if (angle > 135f || angle < -135f)
        {
            animationName = "Forward";
        }
        else if (angle >= -135f && angle <= -45f)
        {
            animationName = "Right";
        }

        ChangeAnimation(animationName);
    }

    private void ChangeAnimation(string animation, float crossfadeTime = 0.1f)
    {
        if (animation != currentAnimation)
        {
            currentAnimation = animation;
            animator.CrossFade(animation, crossfadeTime);
        }
    }

    private void Died()
    {
        if (isDeathAnimation) return;

        isLive = false;
        ChangeAnimation("Death");
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation()
    {
        isDeathAnimation = true;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        Death?.Invoke();
    }

    private void SpeedIncrement(float value)
    {
        speed += value;
    }

    public float Speed => speed;
}
