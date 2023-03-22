using UnityEngine;

public class AudioOnOff : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isMuted = false;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
        if(isMuted)
        {
            Debug.Log("음소거가 켜졌습니다.");
        }
        else
        {
            Debug.Log("음소거가 꺼졌습니다.");
        }
    }
}
