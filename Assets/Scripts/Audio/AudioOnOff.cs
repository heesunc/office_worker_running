using UnityEngine;

public class AudioOnOff : MonoBehaviour
{
    private AudioPlay audioPlay;

    private void Start()
    {
        audioPlay = GameObject.FindObjectOfType<AudioPlay>();
    }

    public void ToggleMute()
    {
        audioPlay.ToggleMute();
    }
}
