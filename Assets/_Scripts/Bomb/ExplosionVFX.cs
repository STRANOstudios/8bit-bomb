using System.Collections;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Animator))]
public class ExplosionVFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ExplosionType explosionType;
    [SerializeField] LayerMask layerMaskPlayer;

    private ExplosionPool pool;
    private Animator animator;

    public delegate void ExplosionHit();
    public static event ExplosionHit HitPlayer;

    void Awake()
    {
        pool = ExplosionPool.Instance;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(WaitForAnimation());
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForAnimation());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((layerMaskPlayer & (1 << other.gameObject.layer)) != 0)
        {
            HitPlayer?.Invoke();
        }
    }

    IEnumerator WaitForAnimation()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null;
        }
        pool.ReturnExplosion(explosionType, gameObject);
    }
}
