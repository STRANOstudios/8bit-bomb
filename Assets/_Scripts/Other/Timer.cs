using UnityEngine;

public class Timer : MonoBehaviour
{
    private int second = 0;

    public delegate void Second(int value);
    public static event Second OnSecond = null;

    public delegate void EndTime();
    public static event EndTime Finish = null;

    private void Start()
    {
        if (GameManager.Instance) second = GameManager.Instance.Timer;
        OnSecond?.Invoke(second);
        InvokeRepeating(nameof(CountDown), 1f, 1f);
    }

    private void OnEnable()
    {
        ExplosionVFX.HitPlayer += StopCountDown;
    }

    private void OnDisable()
    {
        ExplosionVFX.HitPlayer -= StopCountDown;
    }

    private void CountDown()
    {
        OnSecond?.Invoke(second);

        if (second <= 0)
        {
            Finish?.Invoke();
            CancelInvoke(nameof(CountDown));
        }

        second--;
    }

    void StopCountDown()
    {
        CancelInvoke(nameof(CountDown));
    }
}
