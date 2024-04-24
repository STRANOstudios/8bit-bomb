using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExplosionPool : MonoBehaviour
{
    [System.Serializable]
    public class ExplosionPrefab
    {
        public ExplosionType type;
        public GameObject prefab;
    }

    public static ExplosionPool Instance { get; private set; }

    [SerializeField] private List<ExplosionPrefab> explosionPrefabs = new();

    private Dictionary<ExplosionType, ObjectPool<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;

        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<ExplosionType, ObjectPool<GameObject>>();

        foreach (ExplosionPrefab explosionPrefab in explosionPrefabs)
        {
            poolDictionary.Add(explosionPrefab.type, new ObjectPool<GameObject>(() => Instantiate(explosionPrefab.prefab)));
        }
    }

    public GameObject GetExplosion(ExplosionType type)
    {
        if (poolDictionary.ContainsKey(type))
        {
            return poolDictionary[type].Get();
        }
        else
        {
            Debug.LogError("Explosion type not found in pool dictionary.");
            return null;
        }
    }

    public void ReturnExplosion(ExplosionType type, GameObject explosion)
    {
        if (poolDictionary.ContainsKey(type))
        {
            explosion.SetActive(false);
            poolDictionary[type].Release(explosion);
        }
        else
        {
            Debug.LogError("Explosion type not found in pool dictionary.");
        }
    }
}

public enum ExplosionType
{
    End,
    Branch,
    Center
}