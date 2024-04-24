using UnityEngine;

public enum SoundType
{
    Explosion,
    PlaceBomb,
    PlayerDie,
    PlayerWalkAD,
    PlayerWalkWS,
    PowerUp,
    NextLevel,
    EnemyDie,
    EnemyWalk,
}

[DisallowMultipleComponent, RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] soundList;

    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType type, float volume = 1f)
    {
        if (Time.timeScale != 0f) instance.audioSource.PlayOneShot(instance.soundList[(int)type], volume);
    }
}
