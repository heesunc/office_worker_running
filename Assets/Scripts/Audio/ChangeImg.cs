using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeImg : MonoBehaviour
{
    public Sprite[] SoundSprite;
    public Image SoundImg;

    private bool sound;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "SoundBtn")
        {
            sound = true;
            if (MuteManager.IsMuted)
                SoundImg.sprite = SoundSprite[1];
            else
                SoundImg.sprite = SoundSprite[0];
        }
        else if(gameObject.name == "EffectSoundBtn")
        {
            sound = false;
            if (MuteManager.EffectIsMuted)
                SoundImg.sprite = SoundSprite[1];
            else
                SoundImg.sprite = SoundSprite[0];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(sound)
        {
            if (MuteManager.IsMuted)
                SoundImg.sprite = SoundSprite[1];
            else
                SoundImg.sprite = SoundSprite[0];
        }
        else if (gameObject.name == "EffectSoundBtn")
        {
            if (MuteManager.EffectIsMuted)
                SoundImg.sprite = SoundSprite[1];
            else
                SoundImg.sprite = SoundSprite[0];
        }
    }
}
