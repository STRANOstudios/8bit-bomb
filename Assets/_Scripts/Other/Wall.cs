using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private GameObject dropPrefab;

    private Animator animator;
    private string currentAnimation;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Destruct()
    {
        ChangeAnimation("Death");
        StartCoroutine(WaitForAnimation());
    }

    private void ChangeAnimation(string animation, float crossfadeTime = 0.1f)
    {
        if (animation != currentAnimation)
        {
            currentAnimation = animation;
            animator.CrossFade(animation, crossfadeTime);
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForEndOfFrame();

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null;
        }

        if (dropPrefab) Instantiate(dropPrefab, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
