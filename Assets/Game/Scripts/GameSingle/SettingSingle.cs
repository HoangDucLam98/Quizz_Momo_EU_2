using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSingle : MonoBehaviour
{
    [SerializeField] private Button close, backHome, musicBtn, soundBtn;
    public GameObject musicOff, soundOff;
    public AudioSource audio;
    
    bool isMuteMusic;
    bool IsMuteMusic
    {
        get => isMuteMusic;
        set
        {
            isMuteMusic = value;
            musicOff.SetActive(isMuteMusic);
            
            if( !isMuteMusic )
                audio.Play();
                else
                audio.Stop();
        }
    }
    
    bool isMuteSound;
    bool IsMuteSound
    {
        get => isMuteSound;
        set
        {
            isMuteSound = value;
            AudioManager.Ins.isMuteSound = isMuteSound;
            soundOff.SetActive(isMuteSound);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IsMuteMusic = false;
        IsMuteSound = false;
        backHome.AddListener(BackHome);
        close.AddListener(() => GamePlaySingle.Ins.IsOpenSetting = false);
        musicBtn.AddListener(ChangeMusic);
        soundBtn.AddListener(ChangeSound);
    }

    public void ChangeMusic()
    {
        IsMuteMusic = !isMuteMusic;
    }
    
    public void ChangeSound()
    {
        IsMuteSound = !isMuteSound;
    }
    
    public void BackHome()
    {
        gameObject.SetActive(false);
        GamePlaySingle.Ins.GoHome();
    }
}
