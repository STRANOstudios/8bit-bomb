using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu = null;

    private InputHandler inputHandler;

    private static bool isPause = false;
    private bool escapeEnable = true;

    private void Start()
    {
        inputHandler = InputHandler.Instance;

        Pause(false);
    }

    private void Update()
    {
        Escape();
    }

    void Escape()
    {
        if (inputHandler.EscapeInput && escapeEnable)
        {
            Pause();
            StartCoroutine(EscapeDelay());
        }
        pauseMenu.SetActive(isPause);
    }

    IEnumerator EscapeDelay(float value = 0.3f)
    {
        escapeEnable = false;
        yield return new WaitForSeconds(value);
        escapeEnable = true;
    }

    public static void Pause(bool value = true)
    {
        isPause = value && !isPause;

        Time.timeScale = isPause ? 0 : 1;
        Cursor.lockState = isPause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPause;
    }
}
