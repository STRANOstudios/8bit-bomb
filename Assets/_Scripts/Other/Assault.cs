using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class Assault : MonoBehaviour
{
    [Header("Assault Settings")]
    [SerializeField] int enemyCount = 10;
    [SerializeField, Min(1)] int rows;
    [SerializeField, Min(1)] int columns;
    [SerializeField] float cellSize = 1f;

    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;

    private List<Vector3> availablePositions = new();

    void Start()
    {
        CalculateGrid();
    }

    private void OnEnable()
    {
        Timer.Finish += Spawn;
    }

    private void OnDisable()
    {
        Timer.Finish -= Spawn;
    }

    private void Spawn()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 tmp = availablePositions[Random.Range(0, availablePositions.Count)];

            Instantiate(enemyPrefab, tmp, Quaternion.identity, transform);
            availablePositions.Remove(tmp);
        }
    }

    void CalculateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (IsCellExcluded(row, col)) continue;
                Vector3 cellPosition = new(transform.position.x + (col * cellSize), transform.position.y + (row * cellSize), enemyPrefab.transform.position.z);

                availablePositions.Add(cellPosition);
            }
        }
    }

    bool IsCellExcluded(int row, int col)
    {
        return col % 2 != 0 && row % 2 != 0;
    }
}
