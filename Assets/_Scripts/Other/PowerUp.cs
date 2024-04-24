using UnityEngine;

public enum PowerUpType
{
    BiggerExplosion,
    MoreBombs,
}

[DisallowMultipleComponent, RequireComponent(typeof(BoxCollider2D))]
public class PowerUp : MonoBehaviour
{
    [Header("PowerUp Settings")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PowerUpType type;

    public delegate void PowerUpEvent(PowerUpType type);
    public static event PowerUpEvent Upgrade = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((playerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Upgrade?.Invoke(type);
            gameObject.SetActive(false);
        }
    }
}
