using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent, RequireComponent(typeof(GameManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] AudioClip music;

    [Header("Levels Settings")]
    [SerializeField] List<Level> levels;

    private AudioSource audioSource;
    private int indexLevel = 0;
    private int leftLife = 2;
    private List<GameObject> enemies = new();
    private int explosionRadius = 1;

    private void Awake()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        #endregion

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music;
    }

    private void OnEnable()
    {
        Health.Death += DecreaseLife;
        Health.Gameover += GameOver;
        Gate.NextLevel += NextLevel;
    }

    private void OnDisable()
    {
        Health.Death -= DecreaseLife;
        Health.Gameover -= GameOver;
        Gate.NextLevel -= NextLevel;
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    private void DecreaseLife()
    {
        leftLife--;
#if UNITY_EDITOR
        SceneManager.LoadScene(levels[indexLevel].Name);
#else
        SceneManager.LoadScene(1); //indexLevel
#endif
    }

    private void NextLevel()
    {
        foreach (var gameObject in enemies)
        {
            if (gameObject.activeSelf) return;
        }

        enemies.Clear();

        indexLevel++;

        if (indexLevel > levels.Count - 1)
        {
            MenuController.ReturnButton();
        }
        else
        {
#if UNITY_EDITOR
            SceneManager.LoadScene(levels[indexLevel].Name);
#else
            SceneManager.LoadScene(1); //indexLevel + 1
#endif
        }
    }

    private void GameOver()
    {
        indexLevel = 0;
        leftLife = 2;
        explosionRadius = 1;
        enemies.Clear();
        MenuController.ReturnButton();
    }

    public int Timer => levels[indexLevel].Timer;

    public int LeftLife => leftLife;

    public List<GameObject> Enemies 
    { 
        get => enemies; 
        set => enemies = value; 
    }

    public Level Level => levels[indexLevel];

    public int ExplosionRadius 
    { 
        get => explosionRadius; 
        set => explosionRadius = value; 
    }
}
