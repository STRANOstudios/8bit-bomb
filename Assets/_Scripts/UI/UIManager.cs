using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _lifes;
    [Space]
    [SerializeField] Health _health;

    private int score = 0;

    private void Start()
    {
        if (GameManager.Instance) _lifes.text = GameManager.Instance.LeftLife.ToString();
    }

    private void OnEnable()
    {
        Score.point += UpdateScore;
        Timer.OnSecond += UpdateTimer;
    }

    private void OnDisable()
    {
        Score.point -= UpdateScore;
        Timer.OnSecond -= UpdateTimer;
    }

    void UpdateScore(int value)
    {
        score += value;
        _score.text = score.ToString("D6");
    }

    private void UpdateTimer(int value)
    {
        _timer.text = value.ToString("D3");
    }
}
