using UnityEngine;

public class PlayWalk : MonoBehaviour
{
    public void PlaySoundWS()
    {
        SoundManager.PlaySound(SoundType.PlayerWalkWS);
    }

    public void PlaySoundAD()
    {
        SoundManager.PlaySound(SoundType.PlayerWalkAD);
    }
}
