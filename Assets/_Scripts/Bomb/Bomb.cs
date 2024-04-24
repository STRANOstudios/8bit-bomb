using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField, Min(0f)] float explosionDelay = 1f;
    [SerializeField] LayerMask obstacleLayer;

    private BombPool bombPool;
    private ExplosionPool explosionPool;
    private int explosionRadius = 1;

    void Start()
    {
        Inizialization();
    }

    private void OnEnable()
    {
        Inizialization();
    }

    void Inizialization()
    {
        Explosion();

        explosionRadius = ShootingManager.explosionRadius;

        bombPool = BombPool.Instance;
        explosionPool = ExplosionPool.Instance;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Collider2D>().isTrigger = false;
    }

    void Explosion()
    {
        SoundManager.PlaySound(SoundType.PlaceBomb);
        StartCoroutine(ExplodeAfterDelay(explosionDelay));
    }

    IEnumerator ExplodeAfterDelay(float value)
    {
        yield return new WaitForSeconds(value);
        Explode();
    }

    void Explode()
    {
        SoundManager.PlaySound(SoundType.Explosion);
        CheckForObstacle(Vector2.up);
        CheckForObstacle(Vector2.down);
        CheckForObstacle(Vector2.right);
        CheckForObstacle(Vector2.left);

        Spawn(ExplosionType.Center, transform.position, Quaternion.identity);

        if (bombPool) bombPool.ReturnBomb(gameObject);
    }

    void CheckForObstacle(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, explosionRadius, obstacleLayer);

        if (hit.collider)
        {
            switch (hit.collider.gameObject.layer)
            {
                case 9: //wall destructible
                    hit.collider.gameObject.GetComponent<Wall>().Destruct();
                    break;
            }
        }

        Debug.DrawRay(transform.position, direction * explosionRadius, Color.red, 5f);

        if (hit.collider != null && hit.distance < 1) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        int tmp = (int)hit.distance == 0 ? explosionRadius : (int)hit.distance;

        Vector2 spawnPosition = (Vector2)transform.position + (direction * tmp);

        for (int i = 1; i < tmp; i++)
        {
            Vector2 spawnPoint = (Vector2)transform.position + (direction * i);
            Spawn(ExplosionType.Branch, spawnPoint, rotation);
        }

        Spawn(ExplosionType.End, spawnPosition, rotation);
    }

    void Spawn(ExplosionType explosionType, Vector2 spawnPosition, Quaternion rotation)
    {
        GameObject explosionEnd = explosionPool.GetExplosion(explosionType);
        explosionEnd.transform.SetPositionAndRotation(spawnPosition, rotation);
        explosionEnd.SetActive(true);
    }
}
