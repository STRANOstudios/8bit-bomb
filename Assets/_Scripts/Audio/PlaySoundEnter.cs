using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private SoundType soundType;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.PlaySound(soundType, volume);
    }
}

public class PlaySoundExit : StateMachineBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private SoundType soundType;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.PlaySound(soundType, volume);
    }
}