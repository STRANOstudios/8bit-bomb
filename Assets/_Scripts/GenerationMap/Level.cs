using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject
{
    [Header("Level Settings")]
    [SerializeField] string level;
    [SerializeField, Min(1)] int timer;

    [SerializeField] List<Obstacles> obstacles = new();
    [SerializeField] List<Enemies> enemies = new();
    [SerializeField] List<PowerUps> powerUps = new();

    [SerializeField] GameObject gatePrefab;

    public string Name => level;
    public int Timer => timer;
    public List<Obstacles> Obstacles => obstacles;
    public List<Enemies> Enemies => enemies;
    public List<PowerUps> PowerUps => powerUps;
    public GameObject Gate => gatePrefab;
}

[Serializable]
public class Obstacles
{
    public GameObject prefab;
    public int count;
}

[Serializable]
public class Enemies
{
    public GameObject prefab;
    public int count;
}

[Serializable]
public class PowerUps
{
    public GameObject prefab;
    public int count;
}