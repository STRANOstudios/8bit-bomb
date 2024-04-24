using System.Collections;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(CircleCollider2D))]
public class Health : MonoBehaviour
{
    [SerializeField, Min(0)] private int leftLife = 2;
    [SerializeField] private LayerMask layerMaskEnemy;

    private bool filter = true;

    public delegate void Dead();
    public static event Dead Death = null;
    public static event Dead Gameover = null;
    public static event Dead HitEnemy = null;

    private void Start()
    {
        if (GameManager.Instance) leftLife = GameManager.Instance.LeftLife;
    }

    private void OnEnable()
    {
        PlayerManager.Death += Died;
    }

    private void OnDisable()
    {
        PlayerManager.Death -= Died;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((layerMaskEnemy & (1 << other.gameObject.layer)) != 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            HitEnemy?.Invoke();
        }
    }

    private void Died()
    {
        leftLife--;
        if (leftLife < 0 && filter)
        {
            StartCoroutine(Delay());
            Gameover?.Invoke();
            return;
        }

        Death?.Invoke();
    }

    IEnumerator Delay(float delay = 0.3f)
    {
        filter = false;
        yield return new WaitForSeconds(delay);
        filter = true;
    }

    public int LeftLife => leftLife;
}
