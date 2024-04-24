using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(BoxCollider2D))]
public class Gate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask playerLayer;

    public delegate void Event();
    public static event Event NextLevel = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            NextLevel?.Invoke();
        }
    }
}
