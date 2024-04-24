using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ShootingManager : MonoBehaviour
{
    [Header("Shooting Manager")]
    [SerializeField, Min(1)] private int bombMagazine = 1;

    private List<Vector3> bombsPosition = new();
    private InputHandler inputHandler;
    private BombPool bombPool;
    private bool canShoot = true;

    public static int explosionRadius = 1;

    private void Start()
    {
        explosionRadius = GameManager.Instance.ExplosionRadius;
        inputHandler = InputHandler.Instance;
        bombPool = BombPool.Instance;
    }

    private void Update()
    {
        Shoot();
    }

    private void OnEnable()
    {
        BombPool.Restore += BombRestore;
        PowerUp.Upgrade += UpGrade;
    }

    private void OnDisable()
    {
        BombPool.Restore -= BombRestore;
        PowerUp.Upgrade -= UpGrade;
    }

    private void Shoot()
    {
        if (bombMagazine < 1) return;
        if (inputHandler.BombInput && canShoot)
        {
            Vector3 vector3 = new(Mathf.Floor(transform.position.x) + 0.5f, Mathf.Floor(transform.position.y) + 0.5f, 0f);

            if (bombsPosition.Contains(vector3)) return;

            bombsPosition.Add(vector3);

            GameObject tmp = bombPool.GetBomb();
            tmp.transform.position = vector3;
            tmp.SetActive(true);

            bombMagazine--;

            StartCoroutine(DelayBomb());
        }
    }

    IEnumerator DelayBomb(float delay = 0.3f)
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    void BombRestore()
    {
        bombMagazine++;
        if (bombsPosition.Count > 0) bombsPosition.RemoveAt(0);
    }

    void UpGrade(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.BiggerExplosion:
                explosionRadius++;
                GameManager.Instance.ExplosionRadius = explosionRadius;
                break;
            case PowerUpType.MoreBombs:
                bombMagazine++;
                break;
        }
    }
}
