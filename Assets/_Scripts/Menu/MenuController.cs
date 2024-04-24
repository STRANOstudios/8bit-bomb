using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    [SerializeField, Tooltip("The name of the scene to be loaded")] private string sceneToBeLoad;

    public delegate void Resume();
    public static event Resume resume;

    #region Menu Buttons

    public void PlayButton()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(sceneToBeLoad);
#else
        SceneManager.LoadScene(1);
#endif
        if (GetSavedInt("MaxScore") == 0) return;

        PlayerPrefs.SetInt("MaxScore", 0);
        PlayerPrefs.SetInt("LelfLife", 0);
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static void ReturnButton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#if UNITY_EDITOR
        SceneManager.LoadScene("MainMenu");
#else
        SceneManager.LoadScene(0);
#endif
    }

    public void ResumeButton()
    {
        LevelManager.Pause();
    }

    #endregion

    float GetSavedInt(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key);
        return 0;
    }
}