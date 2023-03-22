using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    public void SetLevel()
    {
        mixer.SetFloat("Music", Mathf.Log10(slider.value)*20);
    }
}