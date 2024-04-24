using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Generator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(1)] int rows;
    [SerializeField, Min(1)] int columns;
    [SerializeField] float cellSize = 1f;

    private Level level;
    private List<Vector3> availablePositions = new();
    private List<Vector3> notAvailablePositions = new();
    private List<GameObject> enemies = new();

    private void Start()
    {
        Generation();
    }

    void Generation()
    {
        CalculateGrid();
        level = GameManager.Instance.Level;

        Obstacles();
        PowerUp();
        Gate();
        Enemies();
    }

    #region Spawning

    void Obstacles()
    {
        foreach (var obstacle in level.Obstacles)
        {
            for (int i = 0; i < obstacle.count; i++)
            {
                Vector3 tmp = availablePositions[Random.Range(0, availablePositions.Count)];

                Instantiate(obstacle.prefab, new(tmp.x, tmp.y, obstacle.prefab.transform.position.z), Quaternion.identity, transform);
                notAvailablePositions.Add(tmp);
                availablePositions.Remove(tmp);
            }
        }
    }

    void PowerUp()
    {
        if (notAvailablePositions.Count <= 0) return;
        foreach (var powerUp in level.PowerUps)
        {
            for (int i = 0; i < powerUp.count; i++)
            {
                Vector3 tmp = notAvailablePositions[Random.Range(0, notAvailablePositions.Count)];

                Instantiate(powerUp.prefab, new(tmp.x, tmp.y, powerUp.prefab.transform.position.z), Quaternion.identity, transform);
                notAvailablePositions.Remove(tmp);
            }
        }
    }

    void Gate()
    {
        Vector3 tmp = notAvailablePositions[Random.Range(0, notAvailablePositions.Count)];

        Instantiate(level.Gate, new(tmp.x, tmp.y, level.Gate.transform.position.z), Quaternion.identity, transform);
        notAvailablePositions.Remove(tmp);
    }

    void Enemies()
    {
        foreach (var enemy in level.Enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                Vector3 tmp = availablePositions[Random.Range(0, availablePositions.Count)];

                GameObject enemyObj = Instantiate(enemy.prefab, new(tmp.x, tmp.y, enemy.prefab.transform.position.z), Quaternion.identity, transform);
                enemies.Add(enemyObj);
                availablePositions.Remove(tmp);
            }
        }
        GameManager.Instance.Enemies = enemies;
    }

    #endregion

    void CalculateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (row == rows - 1 && col == 0) continue;
                if (row == rows - 1 && col == 1) continue;
                if (row == rows - 2 && col == 0) continue;
                if (IsCellExcluded(row, col)) continue;

                Vector3 cellPosition = new(transform.position.x + (col * cellSize), transform.position.y + (row * cellSize), 0f);

                availablePositions.Add(cellPosition);
            }
        }
    }

    bool IsCellExcluded(int row, int col)
    {
        return col % 2 != 0 && row % 2 != 0;
    }
}

