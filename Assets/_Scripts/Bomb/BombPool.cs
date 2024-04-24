using UnityEngine;
using UnityEngine.Pool;

public class BombPool : MonoBehaviour
{
    [Header("Bomb Pool")]
    [SerializeField] private GameObject explosionPrefab;

    private ObjectPool<GameObject> explosionPool;

    public static BombPool Instance { get; private set; }

    public delegate void BombRestore();
    public static event BombRestore Restore = null;

    private void Awake()
    {
        Instance = this;

        InitializePool();
    }

    private void InitializePool() => explosionPool = new ObjectPool<GameObject>(() => Instantiate(explosionPrefab));

    public GameObject GetBomb()
    {
        return explosionPool.Get();
    }

    public void ReturnBomb(GameObject explosion)
    {
        explosion.SetActive(false);
        explosion.GetComponent<Collider2D>().isTrigger = true;
        explosionPool.Release(explosion);
        Restore?.Invoke();
    }
}
